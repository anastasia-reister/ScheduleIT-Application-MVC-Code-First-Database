//if the user exists
            if (user != null)
            {
                bool newMessages = db.Messages.Where(m => m.Recipient.Id == user.Id).Where(n => n.UnreadMessage == true).Any();// does the user have any messages
                var openWorkTimeEvents = db.WorkTimeEvents.Where(u => u.Id == user.Id).Where(e => e.End == null).OrderBy(x => x.Start).ToList(); //shows any events that have NULL in end column
                var todaysWorkTimeEvents = db.WorkTimeEvents.Where(u => u.Id == user.Id).Where(e => e.Start.Day == DateTime.Today.Day).OrderBy(x => x.Start).ToList();//shows any events from today
                todaysWorkTimeEvents.AddRange(openWorkTimeEvents); //appends both open events and events from today to one list
                var uniqueTodaysWorkTimeEvents = todaysWorkTimeEvents.Distinct().ToList(); // gets rid of repeating items(if an event is open AND is from today)
                
                bool ClockedIn = db.WorkTimeEvents.Where(x => x.Id == user.Id && !x.End.HasValue).Any();
                return Json(new WorkTimeEventCreateViewModel(true, ClockedIn, newMessages, uniqueTodaysWorkTimeEvents), JsonRequestBehavior.AllowGet);
            }