using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Components.Services;

namespace TaskBase.Components.Views.Shared.Components.SearchMember
{
    [ViewComponent(Name = "SearchMember")]
    public class SearchMemberViewComponent : ViewComponent
    {
        private readonly IAuthService _authService;

        public SearchMemberViewComponent(IAuthService authService)
        {
            _authService = authService;
        }

        public List<UserModel> Users { get; set; } = new List<UserModel>();


        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (Users is null) Users = new();

            Users = await _authService.GetMembersAsync();

            return await Task.FromResult(View(Users));
        }
    }
}
