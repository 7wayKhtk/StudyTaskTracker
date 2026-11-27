using StudyTaskTracker.WebApi.Models;

namespace StudyTaskTracker.WebApi.Data;

public interface ITaskRepository
{
    IReadOnlyList<StudyTask> ListForOwner(int ownerId);
    StudyTask? FindForOwner(int ownerId, int id);
    void Add(StudyTask task);
    bool Delete(StudyTask task);
    bool SaveChanges();
}
