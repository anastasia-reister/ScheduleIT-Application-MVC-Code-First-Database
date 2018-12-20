

//Task:Changeset 435: Add a dropdown list showing years starting with the year of the Payperiod



        //Controller gets the year of the First Payperiod from the db and passes it to the view model
          var payPeriod = db.PayPeriods.FirstOrDefault();
            var year = payPeriod.StartDate.Year;
            EventListVm EVM = new EventListVm(year, workTime: Sorting.ToList(), user: user, ClockedIn: ClockedInStatus);
            return View(EVM);


 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //ViewModel has a property First year

        /// <summary>
        /// Year of the the first PayPeriod in the DB
        /// </summary>
        public int FirstYear { get; set; }


        //overloaded viewModel constructor
        public EventListVm(int FirstYear, List<WorkTimeEvent> workTime, ApplicationUser user, bool ClockedIn)
            : this( workTime, user, DateTime.Now.AddDays(-14), DateTime.Today.AddDays(1).AddSeconds(-1), ClockedIn)
        {
            this.FirstYear = FirstYear;
        }


        
 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //in the View with the help of razor a list of years is generated started from the first year(property of the model)
        <div class="col-sm-2">
            <select name="PayPeriod" class="form-control" style="margin-bottom:5px" id="pp-year">
                @{for (int i = Model.FirstYear; i <= DateTime.Now.Year; i++)
                    {
                        if (i == DateTime.Now.Year)
                        {
                            <option selected="selected">@i</option>
                        }
                        else
                        {
                            <option>@i</option>}
                    }
                }
                }
            </select>
        </div>



       

        //


 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////