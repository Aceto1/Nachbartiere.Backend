using Auth0.ManagementApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nachbartiere.Backend.Database;
using Nachbartiere.Backend.Database.Entities;
using Nachbartiere.Backend.Manager;
using Nachbartiere.Backend.Model;

namespace Nachbartiere.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController: ControllerBase
    {
        [HttpPost]
        [Route("create")]
        [AllowAnonymous]
        public ActionResult CreateUser([FromBody] UserCreateArgument userCreateArgument)
        {
            using var ctx = new DatabaseContext();

            if(!userCreateArgument.AcceptPrivacyNotice)
                return BadRequest("Die Datenschutzbestimmungen müssen akzeptiert werden.");

            var token = ctx.InviteTokens.SingleOrDefault(m => m.Token == userCreateArgument.Token);

            if (token == null)
                return BadRequest("Einladungscode ungültig.");

            var getUsersTask = Auth0Manager.Client.Users.GetUsersByEmailAsync(userCreateArgument.Email.ToLowerInvariant());

            getUsersTask.Wait();

            if (getUsersTask.Result.Count > 0)
                return BadRequest("Benutzer mit dieser Mail-Adresse existiert bereits.");

            var testgroup = ctx.UsersToTestGroups.Count(m => m.TestGroup == 0) <= ctx.UsersToTestGroups.Count(m => m.TestGroup == 1) ? 0 : 1;

            UserCreateRequest userCreateRequest = new UserCreateRequest
            {
                Email = userCreateArgument.Email,
                Password = userCreateArgument.Password,
                Connection = "Username-Password-Authentication",
                VerifyEmail = true,
                UserName = userCreateArgument.Nickname,
                NickName = userCreateArgument.Nickname,
                AppMetadata = new AppMetaData()
                {
                    TestGroup = testgroup
                }
            };

            var userCreationTask = Auth0Manager.Client.Users.CreateAsync(userCreateRequest);

            try
            {
                userCreationTask.Wait();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            if (!userCreationTask.IsCompletedSuccessfully)
                return StatusCode(500);

            var userToTestGroup = new UserToTestGroup()
            {
                UserId = userCreationTask.Result.UserId,
                TestGroup = testgroup
            };
            ctx.UsersToTestGroups.Add(userToTestGroup);
            
            ctx.SaveChanges();

            return Ok();
        }
    }
}
