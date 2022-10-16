using Microsoft.AspNetCore.Mvc;
using Pantree.Data.Models;
using Pantree.Data.Models.Contracts;
using Pantree.Helpers;
using Pantree.Helpers.Enums;
using Pantree.Helpers.Extentions;
using Pantree.Services;

namespace Pantree.Store.Controllers
{
    public class AccountController : BaseController<AccountController>
    {
        public IActionResult Index(string? redirect, int? section)
        {
            if (User.IsAuthenticated)
            {
                section ??= 1;

                var model = new AccountMain();
                model.Section = (AccountSections)section;

                switch (model.Section)
                {
                    case AccountSections.Details:
                        model.CurrentUser = User.UserDetails;
                        break;
                    case AccountSections.Friends:
                        model.FriendMasterView = FriendPartialModel();
                        break;
                }

                return View(model);
            }
            else
            {
                return View("SignIn", new AccessMain(redirect));
            }
        }

        [HttpPost]
        public IActionResult SignIn(SignInSubmit submitted)
        {
            var user = new UserService(submitted.Username, submitted.Password, AppConfig.ConnectionString);
            user.UserModel.RememberMe = submitted.RememberMe;

            if (user.UserModel.IsAuthenticated)
            {
                UserMethods.PantreeUser(user.UserModel, HttpContext, AppConfig.CookieKey);
                return View("Redirect", submitted.Redirect ?? "Account/Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult SignUp(SignUpSubmit submitted)
        {
            var user = new UserService(AppConfig.ConnectionString);
            user.CreateUser(submitted.Username, submitted.Password, submitted.Name, submitted.EmailAddress);

            if (user.UserModel.IsAuthenticated)
            {
                UserMethods.PantreeUser(user.UserModel, HttpContext, AppConfig.CookieKey);
                return View("Redirect", submitted.Redirect ?? "Account/Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        new public IActionResult SignOut()
        {
            ViewBag.User = UserMethods.PantreeUser(new User(), HttpContext, AppConfig.CookieKey);
            return View("Redirect", "Home/Index");
        }

        public IActionResult LoadProfileImage()
        {
            var file = User.UserDetails.ProfileImage.ProfileImage;
            return File(file, "image/jpg", $"ProfileImage_Pantree{RandomString.GetRandomString(12)}.jpg");
        }

        public IActionResult Redirect()
        {
            return View();
        }

        //--------------------------------------------------------------------------------------------

        [HttpPost]
        public IActionResult Details()
        {
            return PartialView("_Details", User.UserDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateDetails(UserEdit details)
        {
            var service = new UserService(AppConfig.ConnectionString);
            details.UserID = User.UserID;
            var user = service.UpdateUserDetails(HttpContext, details, User, AppConfig.CookieKey);

            return PartialView("_Details", user);
        }

        //--------------------------------------------------------------------------------------------

        [HttpPost]
        public IActionResult ProfileImage()
        {
            return PartialView("_ProfileImage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadProfileImage(IFormFile profileImage)
        {
            ImageService service = new ImageService(AppConfig.ConnectionString);

            if (User.UserDetails.ProfileImage == null)
            {
                await service.AddProfileImage(User.UserID, profileImage, $"{User.UserDetails.DisplayName}'s Profile Image");
            }
            else
            {
                service.UpdateProfileImage(User.UserDetails.ProfileImage.ProfileImageID, profileImage, $"{User.UserDetails.DisplayName}'s Profile Image");
            }

            UserService userService = new UserService(AppConfig.ConnectionString);
            userService.UpdateUserCookies(HttpContext, User, AppConfig.CookieKey);

            return PartialView("_ProfileImage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveProfileImage()
        {
            ImageService service = new ImageService(AppConfig.ConnectionString);
            service.RemoveProfileImage(User.UserDetails.ProfileImage.ProfileImageID);

            UserService userService = new UserService(AppConfig.ConnectionString);
            userService.UpdateUserCookies(HttpContext, User, AppConfig.CookieKey);

            return PartialView("_ProfileImage");
        }

        //--------------------------------------------------------------------------------------------

        private FriendMasterView FriendPartialModel()
        {
            var service = new UserService(AppConfig.ConnectionString);

            var model = new FriendMasterView
            {
                Friends = service.GetFriends(User.UserID),
                Search = new FriendSearch()
            };

            return model;
        }

        [HttpPost]
        public IActionResult Friends()
        {
            var model = FriendPartialModel();
            return PartialView("_Friends", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchUsers(FriendSearch search)
        {
            var service = new UserService(AppConfig.ConnectionString);
            search.Results = service.SearchUsers(search.SearchQuery, User.UserID);

            return PartialView("_Friends_Search", search);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendFriendRequest(int UserID)
        {
            var service = new UserService(AppConfig.ConnectionString);
            service.AddFriendRequest(UserID, User.UserID);

            var model = FriendPartialModel();
            return PartialView("_Friends", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AcceptFriend(int UserFriendID)
        {
            var service = new UserService(AppConfig.ConnectionString);
            service.UpdateUserFriendStatus(UserFriendID, User.UserID, FriendStatus.Accepted);

            var userService = new UserService(AppConfig.ConnectionString);
            User.UserDetails = userService.UpdateUserCookies(HttpContext, User, AppConfig.CookieKey);

            var model = FriendPartialModel();
            return PartialView("_Friends", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFriend(int UserFriendID)
        {
            var service = new UserService(AppConfig.ConnectionString);
            service.RemoveUserFriend(UserFriendID);

            var userService = new UserService(AppConfig.ConnectionString);
            User.UserDetails = userService.UpdateUserCookies(HttpContext, User, AppConfig.CookieKey);

            var model = FriendPartialModel();
            return PartialView("_Friends", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BlockFriend(int UserFriendID)
        {
            var service = new UserService(AppConfig.ConnectionString);
            service.UpdateUserFriendStatus(UserFriendID, User.UserID, FriendStatus.Blocked);

            var userService = new UserService(AppConfig.ConnectionString);
            User.UserDetails = userService.UpdateUserCookies(HttpContext, User, AppConfig.CookieKey);

            var model = FriendPartialModel();
            return PartialView("_Friends", model);
        }
    }
}
