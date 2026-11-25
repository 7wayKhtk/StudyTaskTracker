using StudyTaskTracker.Layered.Models;

namespace StudyTaskTracker.Layered.Data;

internal interface ITaskRepository
{
    IReadOnlyList<StudyTask> ListForOwner(int ownerId);
    StudyTask? FindForOwner(int ownerId, int id);
    void Add(StudyTask task);
    void Delete(StudyTask task);
    void SaveChanges();
    int ClaimUnownedTasks(int ownerId);
}
