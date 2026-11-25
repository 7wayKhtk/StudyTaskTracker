-- List every task belonging to Alice, ordered by due date.
-- Change ALICE to the uppercase username you registered.
SELECT t.Id, t.Title, t.Course, t.DueDate, t.IsCompleted
FROM StudyTasks AS t
JOIN Users AS u ON t.OwnerId = u.Id
WHERE u.NormalizedUsername = 'ALICE'
ORDER BY t.DueDate, t.Id;

-- List pending tasks with their owners.
SELECT u.Username, t.Title, t.DueDate
FROM StudyTasks AS t
JOIN Users AS u ON t.OwnerId = u.Id
WHERE t.IsCompleted = 0
ORDER BY t.DueDate, t.Id;

-- Count tasks for every user, including users who have zero tasks.
SELECT u.Username, COUNT(t.Id) AS TaskCount
FROM Users AS u
LEFT JOIN StudyTasks AS t ON t.OwnerId = u.Id
GROUP BY u.Id, u.Username
ORDER BY u.Username;
