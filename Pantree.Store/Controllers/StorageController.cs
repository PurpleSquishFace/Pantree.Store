using Microsoft.AspNetCore.Mvc;
using Pantree.Data.Models.Contracts;
using Pantree.Helpers;
using Pantree.Helpers.Enums;
using Pantree.Services;

namespace Pantree.Store.Controllers
{
    [AuthorizedUser]
    public class StorageController : BaseController<StorageController>
    {
        public IActionResult Index(int? locationID, int? storeID)
        {
            if (locationID != null & User.UserDetails.Locations.Find(i => i.LocationID == locationID) == null) return RedirectToAction("Index");

            var model = new StorageMain(User.UserID, User.UserDetails.Locations, LookupService.AcceptedFriends(User.UserDetails.Friends), locationID)
            {
                Search = (locationID == null & storeID == null),
                SelectedStoreID = storeID
            };

            return View(model);
        }

        //--------------------------------------------------------------------------------------------

        [HttpPost]
        public IActionResult Search()
        {
            var model = new StorageMain(User.UserID, User.UserDetails.Locations, LookupService.AcceptedFriends(User.UserDetails.Friends));
            model.Search = true;
            return PartialView("Index", model);
        }

        [HttpPost]
        public IActionResult SearchStorage(StorageSearchMain search)
        {
            var storageService = new StorageService(AppConfig.ConnectionString);
            search.Results = storageService.Search(search.SearchQuery, User.UserDetails.UserID);

            return PartialView("_Search", search);
        }

        //--------------------------------------------------------------------------------------------

        [HttpPost]
        public IActionResult Location(int LocationID)
        {
            var model = new StorageMain(User.UserID, User.UserDetails.Locations, LookupService.AcceptedFriends(User.UserDetails.Friends), LocationID);

            return PartialView("Index", model);
        }

        [HttpPost]
        public IActionResult LocationAdd()
        {
            var model = new StorageMain(User.UserID, User.UserDetails.Locations, LookupService.AcceptedFriends(User.UserDetails.Friends));
            model.AddLocation = true;
            return PartialView("Index", model);
        }

        [HttpPost]
        public IActionResult AddLocation(LocationAdd location)
        {
            var storageService = new StorageService(AppConfig.ConnectionString);
            var locationID = storageService.AddLocation(location, User.UserID);

            var userService = new UserService(AppConfig.ConnectionString);
            User.UserDetails = userService.UpdateUserCookies(HttpContext, User, AppConfig.CookieKey);

            var model = new StorageMain(User.UserID, User.UserDetails.Locations, LookupService.AcceptedFriends(User.UserDetails.Friends), locationID);

            ModelState.Clear();
            return PartialView("Index", model);
        }

        [HttpPost]
        public IActionResult EditLocationName(LocationNameEdit edit)
        {
            var storageService = new StorageService(AppConfig.ConnectionString);
            storageService.UpdateLocationName(edit);

            var userService = new UserService(AppConfig.ConnectionString);
            User.UserDetails = userService.UpdateUserCookies(HttpContext, User, AppConfig.CookieKey);

            var model = new StorageMain(User.UserID, User.UserDetails.Locations, LookupService.AcceptedFriends(User.UserDetails.Friends), edit.LocationID);

            return PartialView("Index", model);
        }

        [HttpPost]
        public IActionResult EditLocationDescription(LocationDescriptionEdit edit)
        {
            var storageService = new StorageService(AppConfig.ConnectionString);
            storageService.UpdateLocationDescription(edit);

            var userService = new UserService(AppConfig.ConnectionString);
            User.UserDetails = userService.UpdateUserCookies(HttpContext, User, AppConfig.CookieKey);

            var model = User.UserDetails.Locations.Find(i => i.LocationID == edit.LocationID);
            return PartialView("_ViewLocation", model);
        }

        [HttpPost]
        public IActionResult DeleteLocation(LocationDelete delete)
        {
            StorageMain model;
            if (delete.InputMatch)
            {
                var storageService = new StorageService(AppConfig.ConnectionString);
                storageService.DeleteLocation(delete.LocationID, User.UserDetails.UserID);

                var userService = new UserService(AppConfig.ConnectionString);
                User.UserDetails = userService.UpdateUserCookies(HttpContext, User, AppConfig.CookieKey);

                model = new StorageMain(User.UserID, User.UserDetails.Locations, LookupService.AcceptedFriends(User.UserDetails.Friends));
                model.Search = true;
            }
            else
            {
                model = new StorageMain(User.UserID, User.UserDetails.Locations, LookupService.AcceptedFriends(User.UserDetails.Friends), delete.LocationID);
            }

            ModelState.Clear();
            return PartialView("Index", model);
        }

        //--------------------------------------------------------------------------------------------

        [HttpPost]
        public IActionResult Store(int LocationID, int StoreID)
        {
            var storageService = new StorageService(AppConfig.ConnectionString);
            var items = storageService.GetItems(StoreID);

            var model = new StorageMain(User.UserID, User.UserDetails.Locations, LookupService.AcceptedFriends(User.UserDetails.Friends), LocationID)
            {
                SelectedStoreID = StoreID,
                Items = items
            };

            return PartialView("Index", model);
        }

        [HttpPost]
        public IActionResult AddStore(StoreAdd store)
        {
            var storageService = new StorageService(AppConfig.ConnectionString);
            storageService.AddStore(store, User.UserDetails.UserID);

            var userService = new UserService(AppConfig.ConnectionString);
            User.UserDetails = userService.UpdateUserCookies(HttpContext, User, AppConfig.CookieKey);


            ModelState.Clear();

            var model = new StorageMain(User.UserID, User.UserDetails.Locations, LookupService.AcceptedFriends(User.UserDetails.Friends), store.LocationID);

            return PartialView("Index", model);
        }

        [HttpPost]
        public IActionResult EditStoreName(StoreNameEdit edit)
        {
            var storageService = new StorageService(AppConfig.ConnectionString);
            storageService.UpdateStoreName(edit);

            var userService = new UserService(AppConfig.ConnectionString);
            User.UserDetails = userService.UpdateUserCookies(HttpContext, User, AppConfig.CookieKey);

            var model = new StorageMain(User.UserID, User.UserDetails.Locations, LookupService.AcceptedFriends(User.UserDetails.Friends), edit.LocationID);
            model.SelectedStoreID = edit.StoreID;
            model.Items = storageService.GetItems(edit.StoreID);

            return PartialView("Index", model);
        }

        [HttpPost]
        public IActionResult EditStoreDescription(StoreDescriptionEdit edit)
        {
            var storageService = new StorageService(AppConfig.ConnectionString);
            storageService.UpdateStoreDescription(edit);

            var userService = new UserService(AppConfig.ConnectionString);
            User.UserDetails = userService.UpdateUserCookies(HttpContext, User, AppConfig.CookieKey);

            var model = User.UserDetails.Locations.Find(i => i.LocationID == edit.LocationID).Stores.Find(i => i.StoreID == edit.StoreID);
            model.Items = storageService.GetItems(edit.StoreID);

            return PartialView("_ViewStore", model);
        }

        [HttpPost]
        public IActionResult DeleteStore(int LocationID, int StoreID)
        {
            var storageService = new StorageService(AppConfig.ConnectionString);
            storageService.DeleteStore(StoreID, User.UserDetails.UserID);

            var userService = new UserService(AppConfig.ConnectionString);
            User.UserDetails = userService.UpdateUserCookies(HttpContext, User, AppConfig.CookieKey);

            ModelState.Clear();

            var model = new StorageMain(User.UserID, User.UserDetails.Locations, LookupService.AcceptedFriends(User.UserDetails.Friends), LocationID);

            return PartialView("Index", model);
        }

        //--------------------------------------------------------------------------------------------

        [HttpPost]
        public IActionResult RemoveItem(ItemModel param)
        {
            var storageService = new StorageService(AppConfig.ConnectionString);
            var model = storageService.RemoveItem(param.ItemID, param.StoreID, 1);

            return PartialView("_ItemHeading", model);
        }

        [HttpPost]
        public IActionResult AddItem(ItemModel param)
        {
            var storageService = new StorageService(AppConfig.ConnectionString);
            var model = storageService.AddItem(param.ItemID, param.StoreID, 1);

            return PartialView("_ItemHeading", model);
        }

        [HttpPost]
        public IActionResult DeleteItem(ItemModel param)
        {
            var storageService = new StorageService(AppConfig.ConnectionString);
            storageService.DeleteItem(param.ItemID, param.StoreID);

            var store = storageService.GetStore(param.StoreID);
            var items = storageService.GetItems(store.StoreID);

            var model = new StorageMain(User.UserID, User.UserDetails.Locations, LookupService.AcceptedFriends(User.UserDetails.Friends), store.LocationID)
            {
                SelectedStoreID = store.StoreID,
                Items = items
            };

            return PartialView("Index", model);
        }

        //--------------------------------------------------------------------------------------------

        [HttpPost]
        public IActionResult ShareLocation(LocationShareSubmit locationShare)
        {
            var storageService = new StorageService(AppConfig.ConnectionString);
            storageService.ShareLocation(locationShare);

            var userService = new UserService(AppConfig.ConnectionString);
            User.UserDetails = userService.UpdateUserCookies(HttpContext, User, AppConfig.CookieKey);

            ModelState.Clear();

            var model = new StorageMain(User.UserID, User.UserDetails.Locations, LookupService.AcceptedFriends(User.UserDetails.Friends), locationShare.LocationID);

            return PartialView("Index", model);
        }

        [HttpPost]
        public IActionResult UnshareLocation(LocationUnshare locationUnshare)
        {
            var storageService = new StorageService(AppConfig.ConnectionString);
            storageService.UnshareLocation(locationUnshare);

            var userService = new UserService(AppConfig.ConnectionString);
            User.UserDetails = userService.UpdateUserCookies(HttpContext, User, AppConfig.CookieKey);

            ModelState.Clear();

            var model = new StorageMain(User.UserID, User.UserDetails.Locations, LookupService.AcceptedFriends(User.UserDetails.Friends), locationUnshare.LocationID);

            return PartialView("Index", model);
        }
    }
}
