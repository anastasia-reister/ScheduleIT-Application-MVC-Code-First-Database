  
//Task:Changeset 402: On the worktimeevent/index, load the edit page accessed by clicking "edit" on each row, into a modal using ajax.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Modal that receives partial view, Part of the index View

@*Modal for Shift Edit*@
<th>
    <div class="modal" id="MyModal" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h2>Edit Shift</h2>
                </div>
                <div class="modal-body" id="userEditModalBody">
                </div>
                <div class="modal-footer">
                    <button class="btn btn-primary" data-dismiss="modal">close</button>
                </div>
            </div>
        </div>
    </div>
</th>


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//Script fetching partial view and inserting it into modal
<script>
    //Required Dynamic MVC URLs for when scripts are moved to external source
    const EditUserURL = '@Url.Action("Edit","WorkTimeEvent")';
    const PayPeriodURL = '@Url.Action("getPayPeriodDropDown", "PayPeriod")';
</script>
<script>
    //Loads edit modal for changing notes on events worked.
    function userEditModal(EventId) {
        jQuery.ajax({
            'url': EditUserURL,
            'type': 'GET',
            'data': {
                id: EventId
            },
            'success': function (data) {
                document.getElementById("userEditModalBody").innerHTML = data;
                $('#MyModal').modal('show');
            },
            error: function () {
                alert("An error Occurred.  Please try again or contact your system administrator.");
            }

        })
    };



////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//WorkTimeEvent Controller that 
   /// <summary>
        /// Updates only EventID, Start, End Dates and Note. Prevents overposting
        /// </summary>
        /// <param name="Id"></param>
        /// 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        [ActionName("Edit")]
        public ActionResult EditTime(Guid? Id) //gets the worktimeEvent ID
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var EventToUpdate = db.WorkTimeEvents.Find(Id); //searches WorktimeEvent with given Id in the DB
            if (TryUpdateModel(EventToUpdate, "",
         new string[] { "EventId", "Start", "End", "Note" })) //adds updated value to workTimeEvent
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "FirstName");
            return View(Id);
            
            
        }


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//Task:Changeset 411: edit modal accessed from the worktimeevent index displays the start and end dates if the user is an Admin role.


//Partial view to be loaded into modal, looks different depending on the role of the logged in person             
@using (Html.BeginForm("Edit", "WorkTimeEvent", FormMethod.Post))
{
    @Html.AntiForgeryToken()

<div class="form-horizontal">

    @Html.ValidationSummary(true, "", new { @class = "text-danger" })
    @*Passes WorkTimeEvent ID to the Edit Action*@
    @Html.HiddenFor(model => model.Id)
    @if (User.IsInRole("Admin")) //if user role is admin, also loads start and end fields
    {
        <div class="form-group">
            @Html.LabelFor(model => model.Start, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.Start, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.Start, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.End, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.End, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.End, "", new { @class = "text-danger" })
            </div>
        </div>
    }

        <div class="form-group">
            @Html.LabelFor(model => model.Note, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.Note, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.Note, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Save" class="btn btn-default" /> //submits form to Action Edit in WorktimeEvent Controller
            </div>
        </div>
    </div>
}s.Render("~/bundles/jqueryval")
}


