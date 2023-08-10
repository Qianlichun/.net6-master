using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.QLC.Framework.Common.Extension;
using Project.QLC.Framework.Common.Helper;
using Project.QLC.Framework.Model.Dto;
using Project.QLC.Framework.Model.Entity;
using Project.QLC.Framework.Repository.Repository;
using Project.QLC.Framework.Service.Service;
using Project.QLC.Framework.WebAPI.Extension;
using SqlSugar;
using System.IdentityModel.Tokens.Jwt;

namespace Project.QLC.Framework.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TbUserController : ControllerBase
    {
        private readonly ITbUserRepository _tbUserRepository;
        private readonly ISqlSugarClient _client;
        private readonly TokenHelper _tokenHelper;
        private readonly ITbUserService _tbUserService;
        private readonly ILogger<TbUserController> _logger;

        public TbUserController(ITbUserRepository tbUserRepository,
                                ISqlSugarClient client,
                                TokenHelper tokenHelper,
                                ITbUserService tbUserService,
                                ILogger<TbUserController> logger)
        {
            _tbUserRepository = tbUserRepository;
            _client = client;
            _tokenHelper = tokenHelper;
            _tbUserService = tbUserService;
            _logger = logger;
        }

        [HttpPost("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            _logger.LogInformation("NLog-loginfomation");
            _logger.LogWarning("NLog-LogWarning");
            _logger.LogError("NLog-LogError");
            var tuser = await _tbUserRepository.GetByIdAsync(1);
            var result = tuser.MapsterOfProperties<TB_USER, TB_USER>(p=>new TB_USER { 
                ID=p.ID,
                NICK_NAME=p.PASSWORD
            });
            return Ok();
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login()
        {
            var user = await _tbUserRepository.AsQueryable().Where(p => p.ID == 1 && p.PHONE == "12345678910").FirstAsync();
            var user2 = await _client.Queryable<TB_USER>().Where(p => p.ID == 1).FirstAsync();
            var token = _tokenHelper.CreateJwtToken(new TB_USER()
            {
                ID = 1,
                NICK_NAME = "管理员",
                PASSWORD = "222",
                PHONE = "333"
            });
            return Ok(token);
        }
    }
}
