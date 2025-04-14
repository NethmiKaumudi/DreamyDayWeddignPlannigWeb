﻿using DreamyDayWeddingPlanningWeb.Data;
using DreamyDayWeddingPlanningWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DreamyDayWeddingPlanningWeb.Business.Interfaces;

namespace DreamyDayWeddingPlanningWeb.Services
{
    public class WeddingTaskService : IWeddingTaskService
    {
        private readonly ApplicationDbContext _context;

        public WeddingTaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<WeddingTask>> GetTasksByUserIdAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

                return await _context.WeddingTasks
                    .Where(t => t.UserId == userId && !t.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving tasks.", ex);
            }
        }

        public async Task<WeddingTask> GetTaskByIdAsync(int id, string userId)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid task ID.", nameof(id));
                if (string.IsNullOrEmpty(userId))
                    throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

                var task = await _context.WeddingTasks
                    .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId && !t.IsDeleted);

                if (task == null)
                    throw new KeyNotFoundException($"Task with ID {id} not found for the user.");

                return task;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving task with ID {id}.", ex);
            }
        }

        public async Task CreateTaskAsync(WeddingTask weddingTask)
        {
            try
            {
                if (weddingTask == null)
                    throw new ArgumentNullException(nameof(weddingTask), "Wedding task cannot be null.");

                _context.WeddingTasks.Add(weddingTask);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while creating the task. Please check the data and try again.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while creating the task.", ex);
            }
        }

        public async Task UpdateTaskAsync(WeddingTask weddingTask)
        {
            try
            {
                if (weddingTask == null)
                    throw new ArgumentNullException(nameof(weddingTask), "Wedding task cannot be null.");

                var existingTask = await _context.WeddingTasks.FindAsync(weddingTask.Id);
                if (existingTask == null)
                    throw new KeyNotFoundException($"Task with ID {weddingTask.Id} not found.");

                existingTask.TaskName = weddingTask.TaskName;
                existingTask.Deadline = weddingTask.Deadline;
                existingTask.IsCompleted = weddingTask.IsCompleted;

                _context.WeddingTasks.Update(existingTask);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("The task was modified by another user. Please refresh and try again.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the task. Please check the data and try again.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the task.", ex);
            }
        }

        public async Task DeleteTaskAsync(int id, string userId)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid task ID.", nameof(id));
                if (string.IsNullOrEmpty(userId))
                    throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

                var task = await _context.WeddingTasks
                    .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId && !t.IsDeleted);
                if (task == null)
                    throw new KeyNotFoundException($"Task with ID {id} not found for the user.");

                task.IsDeleted = true;
                _context.WeddingTasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while soft-deleting the task.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred while soft-deleting task with ID {id}.", ex);
            }
        }

        public async Task<double> CalculateTaskProgressAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

                var tasks = await _context.WeddingTasks
                    .Where(t => t.UserId == userId && !t.IsDeleted)
                    .ToListAsync();

                if (tasks.Count == 0)
                    return 0;

                var completedTasks = tasks.Count(t => t.IsCompleted);
                return (double)completedTasks / tasks.Count * 100;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while calculating task progress.", ex);
            }
        }
    }
}