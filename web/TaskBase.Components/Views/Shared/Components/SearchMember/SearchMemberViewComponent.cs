using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;

namespace TaskBase.Components.Views.Shared.Components.SearchMember
{
    [ViewComponent(Name = "SearchMember")]
    public class SearchMemberViewComponent : ViewComponent
    {
        public List<UserModel> Users { get; set; } = new List<UserModel>();


        public async Task<IViewComponentResult> InvokeAsync()
        {
            //if (Users is null) Users = new();

            //var users = _userManager.Users.ToList();
            //foreach (User user in users)
            //{
            //if (await _userManager.IsInRoleAsync(user, "Member"))
            //    Users.Add(new UserModel(
            //            user.Id,
            //            user.UserName,
            //            user.AvatarUrl,
            //            null
            //        ));
            //}

            return await Task.FromResult(View(Users));
        }
    }
}
