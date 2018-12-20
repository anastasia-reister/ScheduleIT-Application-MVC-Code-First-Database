

 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Task: Changeset 376: Break modal in create shift page in two columns

//ON the Shifts modal accessed from Employer/Schedule/Create/CreateScheduleForUser, remove the "Edit" title and "Shift" label. Leave the title bar


<div class="modal" id="shiftModal" role="dialog">
    <div class="modal-dialog">
        <div class="modal-content">

            <div class="modal-header">
                <h3 class="modal-title">Shifts</h3>
                @*@<a href="#" class="close" data-dismiss="modal">&times;</a>*@
            </div>
            <div class="modal-body" id="modal2">
                <div class="row">
                    <div class="col-sm-6 col-md-6 col-lg-6">
                        <form id="modalForm">
                            @*Dropdown for shift selection*@
                            @{
                                Html.RenderAction("ShiftModal", "Shift");
                            }

                        </form>
                    </div>
                    <div class="col-sm-6 col-md-6 col-lg-6">
                        <div id="drop" class="">



                        </div>
                    </div>
                </div>
            </div>

           
        </div>
    </div>

</div>


 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////