		
       //Modify the worktimevent/index action so that if a null value is received for the end date, the end date is set to today 
        //Front End
        <div class="col-md-2" style="padding-bottom:5px">
            @Html.EditorFor(x => x.DisplayEndDate, new { htmlAttributes = new { @class = "form-control", type = "date", onblur = "valNotNull()"} })
            
        </div>
    
 
    <script>//Checks if value of the end date is empty. If so sets the date to current date and submits query.
        //If value is not emty, submits query.
        function valNotNull() {
            
            var value =document.getElementById("DisplayEndDate").value;
           
            if (value == "") {
                var today = new Date();
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!
                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd
                }
                if (mm < 10) {
                    mm = '0' + mm
                }
                today = yyyy + '-' + mm + '-' + dd;
                document.getElementById("DisplayEndDate").value = today;
                var x = document.getElementById('DateUpdater');
                x.submit();
            } else {
                var x = document.getElementById('DateUpdater');
                x.submit();
        
            }
           
            
        
            }
    </script>

    //Back End
     //if a null value is received for the end date, the end date is set to today
           [HttpPost]
        public ActionResult Index(EventListVm EVMView, string DateFilterSort, string HoursSorter)
        {
            //if a null value is received for the end date, the end date is set to today
            if (!EVMView.DisplayEndDate.HasValue)
            {
                EVMView.DisplayEndDate = DateTime.Today;
            }