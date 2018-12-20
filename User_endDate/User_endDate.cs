	//Task: Changeset 408: Change end date to N/A for the employee on the _viewuserdetails page if no enddate is set.


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //in the View, evaluate with Razoe if EndDate value is null, set to N/A if it is

        <tr>
            <td class="index-title">@Html.DisplayNameFor(model => model.EndDate)</td>
            @{ if (Model.EndDate == null)
                {
                    <td>N/A</td>
                }
                else
                {
                    <td>@Html.DisplayFor(modelItem => Model.EndDate)</td>
                }
            }
        </tr>

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        //added EndDate nullable parameter and pushed migration

        /// <summary>
        /// User's End Date.
        /// </summary>
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }



//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Controller altered to record EndDate

            public UserDetailsViewModel(ApplicationUser user, string currentRole)
        {
            Id = user.Id; // this is inherited
            FirstName = user.FirstName;
            MiddleName = user.MiddleName;
            LastName = user.LastName;
            HireDate = user.HireDate;
            EndDate = user.EndDate;
            ...