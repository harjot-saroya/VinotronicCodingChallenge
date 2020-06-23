
class Company():
    def __init__(self):
        '''I defined employees as a dictionary, each employee has a hour key that points to their id, I felt like this 
        would be easier since we can search by hours and see how many employees with those hours are available

        '''
        self.availEmp = {}
        self.manPower = 0
        self.numEmp = 0
        self.working = {}
        self.jobQueue = []

    def addEmp(self, hours):
        '''
        (int) -> None
        This function lets the user add a new employee to the available pool, if there is a job in the jobQueue it will
        automatically assign the worker to the job
        '''
        self.numEmp += 1
        if self.availEmp.get(hours) != None:
            self.availEmp[hours] += [self.numEmp]
        else:
            self.availEmp[hours] = [self.numEmp]
        if self.jobQueue != []:
            self.jobHandler(self.jobQueue.pop(0))
        self.manPower = sum(self.availEmp.keys())

    def doublehandler(self, emp, hours):
        '''
        (int,int) -> None
        If an employee with extra hours exist, we delete from the working pool and append their hours back to the available pool
        '''
        key = 0
        ind = 0
        for i in self.availEmp.keys():
            if emp in self.availEmp[i]:
                ind = self.availEmp[i].index(emp)
                key = i
                break
        # Store employee id
        temp = self.availEmp[key][ind]
        del self.availEmp[key]
        # Calculate total hours for this employee
        total = key + hours
        if self.availEmp.get(total) != None:
            self.availEmp[total].append(temp)
        else:
            self.availEmp[total] = [temp]

    def addToPool(self, emp, hours):
        '''
        (int,int) -> None
        This function allows us to add an employee to the available pool
        '''
        if self.availEmp.get(hours) != None:
            self.availEmp[hours].append(emp[0])
        else:
            self.availEmp[hours] = [emp[0]]

    def checkDouble(self, emp):
        '''
        (int) -> bool
        This function checks if an employee exists in both the working pool and available pool
        ie an employee with extra unused hours
        '''
        for i in self.availEmp.keys():
            if emp in self.availEmp[i]:
                return True
            else:
                return False

    def editEmp(self, hours):
        '''
        (int) -> None
        I decided to remove employees from working pool by hours, if there is more than 1 person in a certain hour
        slot then we remove the first one
        '''
        # This var is to check if an employee with additional hours is in the available pool

        double = False

        # Check if input hours exists in working pool
        if self.working.get(hours) != None:
            if self.checkDouble(self.working[hours][0]):
                double = True
            # Multiple employees with input hours
            if len(self.working[hours]) > 1:
                emp = self.working[hours].pop(0)
                # Check for same emp in available pool
                if double:
                    self.doublehandler(emp, hours)
                else:
                    self.addToPool(emp, hours)
            # Single employee with input hours
            else:

                emp = self.working[hours][0]
                # Check for same emp in available pool
                if self.checkDouble(self.working[hours][0]):
                    self.doublehandler(emp, hours)
                else:
                    del self.working[hours]
                    self.addToPool(emp, hours)
        else:
            return 'This hour slot was not found'

    def addWorking(self, time, emp):
        '''
        (int,int)->None
        This function sets a employee to the working pool
        '''
        if self.working.get(time):
            self.working[time].append(emp)
        else:
            self.working[time] = [emp]

    def jobHandler(self, job):
        '''
        (int) -> None
        This function takes in a variable that represents hours, based on the current manpower it will assign employees
        to either work on the job or put the job into the job Queue
        '''

        # Initialize total manpower
        self.manPower = sum(self.availEmp.keys())

        # When all employees are currently working or
        if (self.availEmp == {}):
            self.jobQueue.append(job)

        # Check if input hours already exists in avail pool
        elif self.availEmp.get(job) != None:
            # Multiple employees with input hours
            if len(self.availEmp[job]) > 1:
                if self.working[job]:
                    self.working[job] += self.availEmp[job].pop()
                else:
                    self.working[job] = [self.availEmp[job].pop()]
            # Only one employee
            else:
                if self.working.get(job) != None:
                    self.working[job] += self.availEmp[job].pop()
                    del self.availEmp[job]
                else:
                    self.working[job] = [self.availEmp[job].pop()]
                    del self.availEmp[job]
        # This is the case where we have the collective manpower to do a job, but it is spread out between a bunch of employees
        else:
            # Total hours for needed job
            group = 0
            # Update manpower
            self.manPower = sum(self.availEmp.keys())

            if self.manPower < job:
                self.jobQueue.append(abs(job - self.manPower))
            while group < job:
                # always take the lowest key(employee) from pool and let the total hours
                # add up until it is greater than or equal to the job
                key = min(self.availEmp.keys())
                # If next employee added exceed hours required for job we split one employees hours into both pools
                if group + key > job:
                    diff = abs(group + key - job)
                    temp = self.availEmp[key][0]
                    if len(self.availEmp[key]) == 1:
                        self.addWorking(job, self.availEmp[key][0])
                        del self.availEmp[key]

                    else:
                        self.addWorking(job, self.availEmp[key][0])
                        self.availEmp[key].pop()

                    if self.availEmp.get(diff) != None:
                        self.availEmp[diff].append(temp)
                    else:
                        self.availEmp[diff] = [temp]
                    self.manPower = sum(self.availEmp.keys())
                    break
                # Add to current job's working pool
                group += key
                # Set available workers to work
                if len(self.availEmp[key]) == 1:
                    self.addWorking(key, self.availEmp[key][0])
                    del self.availEmp[key]
                else:
                    self.addWorking(key, self.availEmp[key][0])
                    self.availEmp[key].pop()


if __name__ == '__main__':
    x = Company()
    x.addEmp(1)
    x.addEmp(2)
    x.addEmp(3)
    x.jobHandler(5)

    # x.editEmp(3)
    print(x.availEmp)
    print(x.working)
