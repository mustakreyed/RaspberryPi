using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using RaspberryPIWebApp.Models;

namespace RaspberryPIWebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        [AllowAnonymous]
        public JsonResult UserCheck(string userName, string password)
        {
            var result = SignInManager.PasswordSignIn(userName, password, false, false);

            if (result == SignInStatus.Success)
            {

                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        //http://easyhome.work/account/UserVarify?userName="data"&password="data"
        [AllowAnonymous]
        public JsonResult UserVarify(string userName, string password)
        {
            var result = SignInManager.PasswordSignIn(userName, password, false, false);

            if (result == SignInStatus.Success)
            {
                var user = db.Users.FirstOrDefault(x => x.UserName == userName);
                string userId = user.Id;
                // var pi = db.Pis.Where(p => p.ApplicationUserId == userId).ToList();
                var pi = db.Pis.FirstOrDefault(p => p.ApplicationUserId == userId);
                int piId = pi.PiId;
                // var piPinlist = db.PiPins.Where(p => p.PiId == piId).Select(p => new { p.Room.RoomName,p.PiPinId,p.PinStatus}).ToList();
                var roomlist = db.Rooms.Include(p => p.PiPins)
                    .Where(r => r.PiId == piId).
                    Select(p => new { roomname = p.RoomName, pins = p.PiPins.Select(r => new { id = r.PiPinId, pinNumber = r.PinNumber, status = r.PinStatus, deviceName = r.ApplienceName, deviceLocation = r.Location }) }).ToList();
                return Json(new { pi = roomlist }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { pi = "Invalid User or Passord" }, JsonRequestBehavior.AllowGet);
            }
        }
        [AllowAnonymous]
        public void UserPost(string userName, string password, int piPinId, string status)
        {
            var result = SignInManager.PasswordSignIn(userName, password, false, false);

            if (result == SignInStatus.Success)
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == userName);
                var userPi = db.Pis.FirstOrDefault(p => p.ApplicationUserId == user.Id);
                var pinList = db.PiPins.Where(p => p.PiId == userPi.PiId).ToList();
                var a = pinList.FirstOrDefault(p => p.PiPinId == piPinId);
                a.PinStatus = status;
                db.Entry(a).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                throw new Exception();
            }

        }
        [AllowAnonymous]
        //http://easyhome.work/account/SensorPost?userName=abdullah&password=123456&water=7&temparature=28.5&light=1
        public void SensorPost(string userName, string password, string water, string temparature, string light)
        {
            var result = SignInManager.PasswordSignIn(userName, password, false, false);

            if (result == SignInStatus.Success)
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == userName);
                var userPi = db.Pis.FirstOrDefault(p => p.ApplicationUserId == user.Id);
                var sensor = db.Sensors.FirstOrDefault(s => s.PiId == userPi.PiId);
                if (sensor != null)
                {

                    //SaveSensorData(water, temparature, light, sensor);
                     sensor.Water = water;
                     newsensor.Temparature = temparature;
                     newsensor.Light = light;
                    db.Entry(sensor).State = EntityState.Modified;
                    int status = db.SaveChanges();
                    if (status.Equals(1))
                    {
                        //light Sensor Automated Contol Logic....................
                        var piPin = db.PiPins.FirstOrDefault(p => p.PiId == sensor.PiId && p.PinNumber == sensor.LightControledPin);
                        if (piPin != null)
                        {
                            if (light =="true" )
                            {
                                piPin.PinStatus = "True";
                                db.Entry(piPin).State = EntityState.Modified;
                            }
                            else if (light == "false")
                            {
                                piPin.PinStatus = "False";
                                db.Entry(piPin).State = EntityState.Modified;
                            }
                            else { }
                    
                                int a = db.SaveChanges();
                        }

                        //Water Sensor Automated Contol Logic....................
                        var piPin1 = db.PiPins.FirstOrDefault(p => p.PiId == sensor.PiId && p.PinNumber == sensor.WaterControledPin);
                        //int waterl = Convert.ToInt32(water);
                        if (piPin1 != null)
                        {
                            if (water == "false)
                            {
                                piPin1.PinStatus = "False";
                                db.Entry(piPin1).State = EntityState.Modified;
                            }
                            else if (water == "true)
                            {
                                piPin1.PinStatus = "True";
                                db.Entry(piPin1).State = EntityState.Modified;
                            }
                            else { }
                            
                                int a = db.SaveChanges();
                        }
                        //temparaure Sensor Automated Contol Logic....................
                        //var piPin2 = db.PiPins.FirstOrDefault(p => p.PiId == sensor.PiId && p.PinNumber == sensor.TemparatureControledPin);
                        //if (piPin2 != null)
                        //{
                            decimal temp = Convert.ToInt32(temparature);
                            //if (temp>=35)
                            //{
                                //piPin2.PinStatus = "True";
                                //db.Entry(piPin2).State = EntityState.Modified;
                                //int a = db.SaveChanges();
                            //}
                            //else if (temp<35)
                            //{
                               // piPin2.PinStatus = "False";
                                //db.Entry(piPin2).State = EntityState.Modified;
                                //int a = db.SaveChanges();
                            //}
                            //else { }
                        //}



                    }

                }
                else
                {
                    Sensors newsensor = new Sensors();
                    //SaveSensorData(water, temparature, light, newsensor);
                     sensor.Water = water;
                     newsensor.Temparature = temparature;
                     newsensor.Light = light;
                    newsensor.PiId = userPi.PiId;
                    db.Sensors.Add(newsensor);
                    db.SaveChanges();
                }

            }
            else
            {
                throw new Exception();
            }

        }
        public void SaveSensorData(string water, string temparature, string light, Sensors newsensor)
        {
            //For Water Sensor...........
            if (water == "9")
            {
                newsensor.Water = "overflooded";
            }
            else if (water == "8")
            {
                newsensor.Water = "too high";

            }
            else if (water == "7")
            {
                newsensor.Water = "high";
            }
            else if (water == "6")
            {
                newsensor.Water = "medium high";
            }
            else if (water == "5")
            {
                newsensor.Water = "medium";
            }
            else if (water == "4")
            {
                newsensor.Water = "medium low";
            }
            else if (water == "3")
            {
                newsensor.Water = "low";
            }
            else if (water == "2")
            {
                newsensor.Water = "very low";
            }
            else if (water == "1")
            {
                newsensor.Water = "critical";
            }
            else if (water == "0")
            {
                newsensor.Water = "empty";
            }
            else
            {

            }
            //For Light Sensor...........
            if (light == "0")
            {
                newsensor.Light = "low";
            }
            else if (light == "1")
            {
                newsensor.Light = "mid";
            }
            else if (light == "2")
            {
                newsensor.Light = "high";
            }
            else
            {

            }
            //For Temparature Sensor...........
            newsensor.Temparature = temparature;
        }
        //[AllowAnonymous]
        //public void LightPost(string userName, string password, string light)
        //{
        //    var result = SignInManager.PasswordSignIn(userName, password, false, false);

        //    if (result == SignInStatus.Success)
        //    {
        //        var user = db.Users.FirstOrDefault(u => u.UserName == userName);
        //        var userPi = db.Pis.FirstOrDefault(p => p.ApplicationUserId == user.Id);
        //        var sensor = db.Sensors.FirstOrDefault(s => s.PiId == userPi.PiId);
        //        if (sensor != null)
        //        {
        //            sensor.Light = light;
        //            db.Entry(sensor).State = EntityState.Modified;
        //            db.SaveChanges();
        //        }
        //        else
        //        {
        //            Sensors newsensor = new Sensors();
        //            newsensor.Temparature = light;
        //            db.Sensors.Add(newsensor);
        //            db.SaveChanges();
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception();
        //    }

        //}
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "UserDashboard");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Name, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "User");
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("GetRoomNames", "PiConfig");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
