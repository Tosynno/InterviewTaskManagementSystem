using Domain;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Dto;
using TaskManager.API.Models;

namespace TaskManager.API.BusinessLogic
{
    public class GeneralClass
    {
        private readonly AppDbContext _dbContext;
        public GeneralClass(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //internal async Task<bool> AuthUser(st)
        internal async Task<string> CreateTask(CreateTaskRequest request)
        {
            Random random = new Random();

            tbl_Task sa = new tbl_Task();   
            sa.TaskName = request.TaskName;
            sa.Description = request.Description;
            sa.Status = 0;
            sa.TaskControlId = $"DocNo{DateTime.UtcNow.ToString("yyyyMMddhhmm")}{random.Next(99999)}";

            await _dbContext.tbl_Tasks.AddAsync(sa);
            await _dbContext.SaveChangesAsync();

            return "Task was successfully Created";
        }

        internal async Task<List<GetTaskDto>> GetAllTask()
        {
            var allres = new List<GetTaskDto>();
            var res = new GetTaskDto();
            var result = await _dbContext.tbl_Tasks.OrderByDescending(c => c.Id).ToListAsync();
            foreach (var item in result)
            {
                res = new GetTaskDto();
                res.TaskControlId = item.TaskControlId;
                res.TaskName = item.TaskName;
                res.Description = item.Description;
                if (item.Status == 0)
                {
                    res.Status = "NOT ASSIGN";
                }
                else
                {
                    res.Status = "ASSIGNED";
                }

                allres.Add(res);
            }
            return allres;
        }

        internal async Task<GetTaskDto> GetAllTask(string TaskControlId)
        {
            var res = new GetTaskDto();
            var result = await _dbContext.tbl_Tasks.FirstOrDefaultAsync(c => c.TaskControlId == TaskControlId);
            if (result != null)
            {
                res.TaskControlId = result.TaskControlId;
                res.TaskName = result.TaskName;
                res.Description = result.Description;
                if (result.Status == 0)
                {
                    res.Status = "NOT ASSIGN";
                }
                else
                {
                    res.Status = "ASSIGNED";
                }
            }
            return res;
        }

        internal async Task<string> UpdateTask(string TaskControlId, CreateTaskRequest request)
        {
            var result = await _dbContext.tbl_Tasks.FirstOrDefaultAsync(c => c.TaskControlId == TaskControlId);
            if (result != null)
            {
                result.TaskName = request.TaskName;
                result.Description = request.Description;
                 _dbContext.tbl_Tasks.Update(result);
                await _dbContext.SaveChangesAsync();

            }

          

          

            return "Task was successfully Created";
        }

        internal async Task<string> AssignTask(string TaskControlId, string Token)
        {
            string Response = "";
            var task = await _dbContext.tbl_Tasks.FirstOrDefaultAsync(c => c.TaskControlId == TaskControlId && c.Status == 0);
            if (task != null)
            {
                var userlog = await _dbContext.tbl_AuditLogs.FirstOrDefaultAsync(c => c.Token == Token);
                if (userlog != null)
                {
                    var user = await _dbContext.tbl_Onboardings.FirstOrDefaultAsync(c => c.Id == userlog.UserId);
                    if (user != null)
                    {
                        tbl_TaskAssign sa = new tbl_TaskAssign();
                        sa.TaskId = task.Id;
                        sa.UserId = user.Id;
                        sa.Status = "ASSIGNED";

                        await _dbContext.tbl_TaskAssigns.AddAsync(sa);
                        await _dbContext.SaveChangesAsync();
                        Response = $"Task was Assign to user  {user.Email}";
                    }
                   
                }
                task.Status = 1;

                 _dbContext.tbl_Tasks.Update(task);
                await _dbContext.SaveChangesAsync();



            }

            return Response;
        }
    
        internal async Task<List<GetTaskDto>> ShowAssignAllTaskByUser(string Token)
        {
            var allres = new List<GetTaskDto>();
            var res = new GetTaskDto();
            var userlog = await _dbContext.tbl_AuditLogs.FirstOrDefaultAsync(c => c.Token == Token);
            if (userlog != null)
            {
                var user = await _dbContext.tbl_Onboardings.FirstOrDefaultAsync(c => c.Id == userlog.UserId);
                if (user != null)
                {
                    var taskuser = await _dbContext.tbl_TaskAssigns.Where(c => c.UserId == user.Id).ToListAsync();
                    foreach (var item in taskuser)
                    {
                        var result = await _dbContext.tbl_Tasks.FirstOrDefaultAsync(c => c.Id == item.TaskId);
                        if (result != null)
                        {
                            res = new GetTaskDto();
                            res.TaskControlId = result.TaskControlId;
                            res.TaskName = result.TaskName;
                            res.Description = result.Description;
                            res.Status = item.Status;

                            allres.Add(res);
                        }
                       
                    }
                }

            }
            return allres;  
        }

        internal async Task<string> AcceptAssignTaskByUser(string Token, AcceptAssignRequest request)
        {
           
            var userlog = await _dbContext.tbl_AuditLogs.FirstOrDefaultAsync(c => c.Token == Token);
            if (userlog != null)
            {
                var user = await _dbContext.tbl_Onboardings.FirstOrDefaultAsync(c => c.Id == userlog.UserId);
                if (user != null)
                {
                    var result = await _dbContext.tbl_Tasks.FirstOrDefaultAsync(c => c.TaskControlId == request.TaskControlId);
                    if (result != null)
                    {
                        var taskuser = await _dbContext.tbl_TaskAssigns.FirstOrDefaultAsync(c => c.UserId == user.Id);
                        if (taskuser != null)
                        {
                            taskuser.Status = "ONGOING TASK";
                            taskuser.StartDate = request.StartDate;
                            taskuser.EndDate = request.EndDate; 
                            _dbContext.Update(taskuser);
                            await _dbContext.SaveChangesAsync();

                        }
                    }
                    
                }

            }
            return "User Accept Task";
        }

        internal async Task<string> CompleteAssignTaskByUser(string Token, CompleteAssignRequest request)
        {

            var userlog = await _dbContext.tbl_AuditLogs.FirstOrDefaultAsync(c => c.Token == Token);
            if (userlog != null)
            {
                var user = await _dbContext.tbl_Onboardings.FirstOrDefaultAsync(c => c.Id == userlog.UserId);
                if (user != null)
                {
                    var result = await _dbContext.tbl_Tasks.FirstOrDefaultAsync(c => c.TaskControlId == request.TaskControlId);
                    if (result != null)
                    {
                        var taskuser = await _dbContext.tbl_TaskAssigns.FirstOrDefaultAsync(c => c.UserId == user.Id && c.EndDate == request.EndDate);
                        if (taskuser != null)
                        {
                            taskuser.Rate = 5;
                            taskuser.Status = "COMPLETE TASK";
                            taskuser.EndDate = request.EndDate;
                            _dbContext.Update(taskuser);
                            await _dbContext.SaveChangesAsync();

                        }
                        else
                        {
                            var taskUser = await _dbContext.tbl_TaskAssigns.FirstOrDefaultAsync(c => c.UserId == user.Id);
                            if (taskUser != null)
                            {
                                taskUser.Rate = 2;
                                taskUser.Status = "COMPLETE TASK";
                                taskUser.EndDate = request.EndDate;
                                _dbContext.Update(taskUser);
                                await _dbContext.SaveChangesAsync();

                            }

                        }
                    }

                }

            }
            return "Task Completed";
        }
  
        internal async Task<List<bool>> IncomingTask()
        {
            var AllResponse = new List<bool>();
            var Response = new bool();
            var task = await _dbContext.tbl_Tasks.Where(c => c.Status == 0).ToListAsync();
            if (task.Count() > 0)
            {
                foreach (var item in task)
                {
                    var userlog = await _dbContext.tbl_TaskAssigns.FirstOrDefaultAsync(c => c.TaskId == item.Id && c.EndDate == DateTime.Now.AddDays(-3));
                    if (userlog != null)
                    {
                        var user = await _dbContext.tbl_Onboardings.FirstOrDefaultAsync(c => c.Id == userlog.UserId);
                        if (user != null)
                        {
                            tbl_TaskAssign sa = new tbl_TaskAssign();
                            sa.TaskId = item.Id;
                            sa.UserId = user.Id;
                            sa.Status = "INCOMING TASK";

                            await _dbContext.tbl_TaskAssigns.AddAsync(sa);
                            await _dbContext.SaveChangesAsync();
                            Response = true;
                            AllResponse.Add(Response);
                        }

                    }
                    else
                    {
                        Response = false;
                        AllResponse.Add(Response);
                    }
                }
              
            }

            return AllResponse;
        }
    }
}
