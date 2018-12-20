//View
//depending on the button pressed variables are assigmned values corresponding to action of Delete or Terminate user

string url;
    if (ViewBag.buttonClicked == "terminate")
    {
        message = "stop employment for";
        actionRoute = "ChangeRole";
        buttonLabel = "Stop";
        url = "TerminateUserURL";
    }
    else
    {
        message = "delete";
        actionRoute = "Delete";
        buttonLabel = "Delete";
        url = "DeleteUserURL2";
    }
}
    <div class="form-group text-center">
        <div class="col-md-offset-1 col-md-10">
            <input type="button" onclick="deleteStopUser(@url)" class="login-button" value="@buttonLabel" />
        </div>
    </div>
}

//route to terminateUser/DeleteUser Action for Ajax post call

const DeleteUserURL2 = '@Url.Action("DeleteUser", "Account", new { area = "" } )';
const TerminateUserURL = '@Url.Action("TerminateUser", "Account", new { area = ""} )';

//script to direct to the right action
  function deleteStopUser(url) {
        var user = $('#UserName').val();
        var pass = $('#PassWord').val();
        var userToChange = $('#userId').val();

        $.post({
        url: url,
        data: {
        'email': user,
        'password': pass,
        'userToChange': userToChange
        },
        success: function (data) {
                if (data == "True") {
                 window.location.reload();
                } else {
                 $('#deleteStopMessage').text("You entered the wrong credentials. Please try again.").css("color", "red");
                 $('#delete-stop-credentialCheck').modal('show');
                }
                },
        error: function () {
               alert('fail');
               }
});
}

//method changes user role to "Terminated" in the DB
        [HttpPost]
        public async Task<bool> TerminateUser(LoginViewModel lvm, string userToDelete)
        {
            
            if (CheckCredentials(lvm))
            {
                using (UserController u = new UserController(UserManager, SignInManager))
                {
                     await u.ChangeRole(userToDelete, "Terminated");
                }}
                return true;
            }
            else
            {
                return false;
            }
        }

//Delete user method existed prior