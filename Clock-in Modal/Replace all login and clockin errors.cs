  //Checks if the user's credentials are valid, if not, prompts an error message
            if (!AccountController.CheckCredentials(lvm))
            {
                ModelState.AddModelError("", "There was a problem with your credentials or account status. Please try again or contact your system administrator if the problem continues.");
                return View("~/Views/Account/Login.cshtml", lvm);
            }



            if (!regex.IsMatch(model.Email))
                {
                    ModelState.AddModelError("Email", "There was a problem with your credentials or account status. Please try again or contact your system administrator if the problem continues.");
                }