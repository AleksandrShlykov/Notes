using Notes.Persistion;

namespace Notes.Tests.Common
{
    public class TestCommandBase : IDisposable
    {
        protected readonly NotesDbContext _context;
        public TestCommandBase()
        {
            _context = NotesContextFactory.Create();
        }

        public void Dispose()
        {
            NotesContextFactory.Destroy(_context);
        }

    }
}
