  


//Task: Changeset 395: Create a confirmation modal for clocking in and out on the login page

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//View: Fields for username and password. Clock-in button connected to a script function
 
                <div class="centerthis">
                    <div class="form-group input-form-group">
                        <div class="col-md-offset-1 col-md-10 mx-auto">
                            <div class="login-uNPassword">
                                @Html.TextBoxFor(m => m.Email, new { @class = "form-control text-center centerthis", placeholder = "username", onchange = "focus(), blur(), UserCheck()", onselect = "UserCheck()", id = "UserName" })
                                @Html.ValidationMessageFor(m => m.Email, "", new { @class = "text-danger", id="username-error-msg" })
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-1 col-md-10 mx-auto">
                            <div class="login-uNPassword">
                                @Html.PasswordFor(m => m.Password, new { @class = "form-control text-center centerthis", placeholder = "password", onchange = "UserCheck(), focus(), blur()", id = "PassWord" })
                                @Html.ValidationMessageFor(m => m.Password, "", new { @class = "text-danger", id = "password-error-msg" })
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group text-center">
                    <div class="col-md-offset-1 col-md-10">
                    <input type="button" class="login-button clockInOutBtn" value="Clock In" onclick="VerifyUserForClock()"  />
                    <input type="submit" class="login-button" value="Log In" />
                </div>
            </div>
      

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



//modal pops up if function verifyUserForClock returns true
 <!-- Modal -->
<div class="modal fade" id="clock-modal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
 
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="checkin-modal-title"></h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
               
                    <textarea class="form-control rounded-0 centerthis" id="modal-note" name = "Note" rows="4" placeholder="This note is for the Clock-In/Out">
        
                    </textarea>
                
            </div>
            <div class="modal-footer">
               
                <input type="submit" class=" login-button clockInOutBtn" value="Clock In" formaction=@Url.Action("Create", "WorkTimeEvent") formmethod="post" />
                <button type="button" class="  login-button " data-dismiss="modal">Cancel</button>
               
            </div>
        </div>
        
    </div>
</div>


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



    //CHECK USER NAME AVAILABILITY
    //redirects username and password values to VerifyUserForClockMEthod in the Account Controller
<script>
    function VerifyUserForClock() {
        var user = $('#UserName').val();
        var pass = $('#PassWord').val();
        $.post({
            url: "@Url.Action("VerifyUserForClock", "Account")",
            data: {
                'Email': user,
                'Password': pass,
                'Note': note,
            },
            success: function (data) {
                
                if (data.verified == false) {
                    $('#ErrorMessages').text("There was a problem with your credentials or account status. Please try again or contact your system administrator if the problem continues.").css("color", "red");
                } else () {
                    $('#clock-modal').modal('show');
                    $('#modal-note').text(data.EventNotes);
                }
                
            },
            error: function () {
                alert('Fail');
            }
        })
    }
   

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//Task: Overload the  the CheckCredentials() in the AccountController so that it also can receive loginviewmodel 

    
 /// <summary>
        /// Overloaded CheckCredentials() method to take LoginViewModel as a parameter
        /// </summary>
        /// <param name="lvm"></param>
        /// <returns></returns>
        public static bool CheckCredentials(LoginViewModel lvm)
        {
           return CheckCredentials(lvm.Email, lvm.Password);
        }


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    //Task:Changeset 405: Update ClockIn modal to be able to send a note to the DB with the event, display the note on clock-out, alter the note on clock-out in DB
    //Task: Changeset 419: add jason object for VerifyUserForClock return if false
    // checks if user is verified and if so loads existing notes associated with the user into modal(if user added a not on clock in and now is clocking out)
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public object VerifyUserForClock(LoginViewModel lvm)
        {

            if (CheckCredentials(lvm))
            {
                var user = db.Users.Where(x => x.UserName == lvm.Email).SingleOrDefault();
                
                var currentWorkTimeEvents = db.WorkTimeEvents.Where(u => u.Id == user.Id).Where(e => e.End == null).OrderBy(x => x.Start).ToList();
                var notes = "";
                foreach (WorkTimeEvent wte in currentWorkTimeEvents)
                {
                    
                    notes = notes + wte.Note;
                }
                return Json(new { verified = true, EventNotes = notes, userFirstName = user.FirstName, userLastName = user.LastName }, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(new { verified = false, EventNotes = "" }, JsonRequestBehavior.AllowGet);
            };

        }





        


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //CREATE A WORKTIMEEVENT       
        
        //Create Method modified to add Note to Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create(LoginViewModel lvm)    // workTimeEvent grabs the login email, password, and remember me
        {
            //Get the current time first thing to ensure there's minimal delay between selecting clockin on the front-end and the time saved in the database
            DateTime? dt = DateTime.Now;
       
            // Checks Db users for email that matches the email user typed in
            ApplicationUser dbUser = db.Users.FirstOrDefault(x => x.Email == lvm.Email);

            
            //Checks if the user's credentials are valid, if not, prompts an error message
            if (!AccountController.CheckCredentials(lvm))
            {
                ModelState.AddModelError("", "There was a problem with your credentials or account status. Please try again or contact your system administrator if the problem continues.");
                return View("~/Views/Account/Login.cshtml", lvm);
            }

            // Check if user is clocked in by checking if any events exist without an end time.
            var notFinishedEvent = db.WorkTimeEvents.FirstOrDefault(x => x.Id == dbUser.Id && !x.End.HasValue);
            //get's note from the LoginViewModel
            var note = lvm.Note;

            // If an event is found that doesn't have an end value, the user is currently clocked in
            if (notFinishedEvent != null)
            {
                //overrites existing(if existed) note in DB
                notFinishedEvent.Note = note;
                //Update the current open event with an end datetime.
                notFinishedEvent.Clockout();
                db.SaveChanges();
                //Create message to be passed to the Login Action for use on Login View
                TempData["message"] = "Successful Clock out at " + DateTime.Now.ToString("h:mm tt");
                return RedirectToAction("Login", "Account");
            }
            else
            {
                //If not clocked in, create a new worktimevent, set it's date to right now and adds the note.
                WorkTimeEvent clockIn = new WorkTimeEvent(dbUser, note, dt);
                db.WorkTimeEvents.Add(clockIn);
                db.SaveChanges();
                //Create message to be passed to the Login Action for use on Login View
                TempData["message"] = "Successful Clock in at " + DateTime.Now.ToString("h:mm tt");
                return RedirectToAction("Login", "Account");
            }
        }    


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
          


        //WorkTimeEvent Constructor modified to accept parameter Note
            public WorkTimeEvent(ApplicationUser employee, string Note, DateTime? dt = null)
        {
            User = employee;          
            EventID = Guid.NewGuid();
            if (dt.HasValue) this.Start = dt.Value;
            else this.Start = DateTime.Now;
            this.Note = Note;
        }