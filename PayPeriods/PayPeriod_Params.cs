     

     //Task: Changeset 423: Update payperiod model with GUID Id type and constructor to prepopulate the ID.

    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //Change id parameter to Guid, pushed migration
        [Key]
        public Guid PayPeriodID { get; set; }
        /// <summary>
        /// Pay period days
        /// </summary>
        /// </summary>
        public DateTime StartDate { get; set; }

    //Add constructor to generate new GUID with every new payperiod record    
        public  PayPeriod()
        {
            PayPeriodID = Guid.NewGuid();
        }
    }
}

 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//Task:Changeset 429: Add initializer to startup.cs that creates a payperiod

//Add a payperiod to the database so that payperiod functionality exists.
            if (!context.PayPeriods.Any())
            {
                var payPeriod = new PayPeriod()
                {
                    PayPeriodLength = 14,
                    DaysUntilPayDay = 7,
                    StartDate = DateTime.Now.AddYears(-2)
                };


                context.PayPeriods.Add(payPeriod);
                context.SaveChanges();
            }
        }



 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //Task:Changeset 435: Add a dropdown list showing years starting with the year of the Payperiod

        <div class="col-md-3" id="pay-period-partial" onchange="populateDatepickers()">
                @{Html.RenderAction("getPayPeriodDropDown", "PayPeriod", new { ppYear = DateTime.Now.Year.ToString() });}
        </div>

        //


        //method to generate payperiods. Calculates payperiods of a given year, with a starting point generated based on the very first payperiod of the company

        ApplicationDbContext db = new ApplicationDbContext();
       
        public List<PayPeriod> generatePayPeriods(int ppYear)
        {
            var firstPP = db.PayPeriods.FirstOrDefault(); //grabs the first payperiod from the db
            var theStartDate = firstPP.StartDate; //grabs the start date of the first payperiod
            var endDate = theStartDate.AddDays(firstPP.PayPeriodLength-1); //calculates end date of the first payperiod based on length of the payperiod

            DateTime currentYearStart = new DateTime(ppYear, 1, 1); // grabs the first day of the year passed to the method
            DateTime currentYearPPStart;
            if (theStartDate > currentYearStart)// if the very first payperiod start date is after this year start date, then current year start date is concidered at that date
            {
                 currentYearPPStart = theStartDate;
            }
            else { //otherwise calculate the total of days from the very first payperiod startdate to this year Jan1 
                int totalDays = (currentYearStart - theStartDate).Days;
                currentYearPPStart = currentYearStart.AddDays(-totalDays % firstPP.PayPeriodLength); //get the remainder of days not amounted to a full payperiod and offset the first day of the year by that number of days
            }                                                                                        // this way all payperiods are calcualted based of the very first pay period

            List<PayPeriod> yearPP = new List<PayPeriod>(); //generate all the payperiods within the needed year and assign them to a list of PayPeriod Objects
            do {
                PayPeriod PP = new PayPeriod();
                PP.StartDate = currentYearPPStart;
                PP.PayPeriodLength = firstPP.PayPeriodLength;
                yearPP.Add(PP);
                currentYearPPStart = currentYearPPStart.AddDays(PP.PayPeriodLength);
            } while (currentYearPPStart <= DateTime.Now && currentYearPPStart.Year < ppYear + 1);
            return yearPP; //return the list of objects
        }



        public ActionResult getPayPeriodDropDown(int ppYear)
            {
               var yearPP = generatePayPeriods(ppYear);
                return PartialView("_PayPeriodDD", yearPP);
            }


            //PayPeriod DropDown List Partial _PayPeriodDD.cshtml

        @model List<ScheduleUsers.Models.PayPeriod>

        <select name="PayPeriod" class="form-control" style="margin-bottom:5px" id="pay-period">
            @{
                foreach (ScheduleUsers.Models.PayPeriod pp in Model)
                {
                    var endDate = pp.StartDate.AddDays(pp.PayPeriodLength);
                    <option id="pay-period-option" data-start="@pp.StartDate.ToString("yyyy-MM-dd")" data-end="@endDate.ToString("yyyy-MM-dd")">@pp.StartDate.ToShortDateString()  - @endDate.ToShortDateString()  </option>
                }

            }
        </select>


        <script>
            function populateDatepickers() {

                var ppEle = $('#pay-period').children("option:selected");
                var ppStart = ppEle.data("start");
                var ppEnd = ppEle.data("end");
                document.getElementById("DisplayBeginDate").value = ppStart;
                //$("#picker-start").datepicker('setDate', ppStart);
                document.getElementById("DisplayEndDate").value = ppEnd;
                //$("#picker-end").datepicker('setDate', ppEnd);
                validateAndSubmitForm();
            };
        </script>