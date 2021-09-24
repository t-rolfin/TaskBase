using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Components.Models;
using TaskBase.Data.Identity;

namespace TaskBase.Components.Views.Shared.Components.SearchMember
{
    [ViewComponent(Name = "SearchMember")]
    public class SearchMemberViewComponent : ViewComponent
    {
        readonly UserManager<User> _userManager;
        public List<UserModel> Users { get; set; }


        public SearchMemberViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
                if (Users is null) Users = new();

                var users = _userManager.Users.ToList();
                foreach (User user in users)
                {
                if (await _userManager.IsInRoleAsync(user, "Member"))
                    Users.Add(new UserModel(
                            user.Id,
                            user.UserName,
                            user.AvatarUrl,
                            null
                        ));
                }

                return View(Users);
        }
    }
}
