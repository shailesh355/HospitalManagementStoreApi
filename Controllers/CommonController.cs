﻿using BaseClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagementApi.Models.BLayer;
using TicketManagementApi.Models.DaLayer;
namespace TicketManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        readonly DlCommon dl = new();
        /// <summary>
        /// Get Indian State List, Default Language is Set to English (value 2)
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        [HttpGet("state/{language?}")]
        public async Task<List<ListValue>> State(LanguageSupported language = LanguageSupported.English)
        {
            List<ListValue> lv = await dl.GetStateAsync(language);
            return lv;
        }
        /// <summary>
        /// Get List of District based on State value, Default Language is Set to English (value 2)
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HttpGet("district/{sid}/{language?}")]
        public async Task<List<ListValue>> District(int sid = (int)StateId.DefaultState, LanguageSupported language = LanguageSupported.English)
        {
            List<ListValue> lv = await dl.GetDistrictAsync(sid, language: language);
            return lv;
        }
        /// <summary>
        /// Get List of Base Departments of a State, Default Language is Set to English (value 2)
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HttpGet("basedepartment/{sid}/{language?}")]
        public async Task<List<ListValue>> BaseDepartment(int sid = (int)StateId.DefaultState, LanguageSupported language = LanguageSupported.English)
        {
            List<ListValue> lv = await dl.GetBaseDepartmentAsync(sid, language: language);
            return lv;
        }
        /// <summary>
        /// Get List of Base Designation of a State, Default Language is Set to English (value 2)
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HttpGet("designation/{sid}/{language?}")]
        public async Task<List<ListValue>> BaseDesignation(int sid = (int)StateId.DefaultState, LanguageSupported language = LanguageSupported.English)
        {
            List<ListValue> lv = await dl.GetDesignationAsync(sid, language: language);
            return lv;
        }
        /// <summary>
        /// Get Common List based on Category
        /// </summary>
        /// <param name="category"></param>
        /// <param name="language">Hindi = 1, English = 2</param>
        /// <returns></returns>
        [HttpGet("CommonList/{category}/{language?}")]
        public async Task<List<ListValue>> GetCommonListPublicAsync(string category, LanguageSupported language = LanguageSupported.English)
        {
            List<ListValue> lv = await dl.GetCommonListAsync(category: category, language: language);
            return lv;
        }
        /// <summary>
        /// Get Common List based on Category
        /// </summary>
        /// <param name="category"></param>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HttpGet("sublist/{category}/{id}/{language?}")]
        public async Task<List<ListValue>> GetSubCommonListPublicAsync(string category, string id, LanguageSupported language = LanguageSupported.English)
        {
            List<ListValue> lv = await dl.GetSubCommonListAsync(category: category, id: id, language: language);
            return lv;
        }
    }
}
