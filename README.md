# Generic Windows Service
Model project to demonstrate all the "good implementations"


#### To Do List

- ✅ Dependency Injection
- ✅ Serilog Logger + Rotating File
- ✅ Cron tab (similar) scheduling system for processes (NCrontab.Signed)
- ✅ Unit tests (xUnit, FluentAssertions, NSubstitute, bogus)
- ✅ Multi-thread processes (manage maximum threads)
- ✅ Manage thread starting of a process, if same process havent finnished in another thread
- 🔲 Semaphore for multithreaded process executoion
- 🔲 Add Factory to spawn diferent processes
- 🔲 Add Few generic processes
- 🔲 Add Integration Tests
- 🔲 Possibility to read adhoc lists
- 🔲 Add Notifier abstarction with possibility to configure multiple notifications (email, etc)
- 🔲 File to pdf conversion (Aspose)
- 🔲 PDF File Merge
- 🔲 Generate a PDF from scratch (QuestPDF)
- 🔲 Email sending (FluentEmail to send, PaperCut SMPT server to receive emails in DEV, not to send emails to actual users for testing purposes :grimacing:)
- 🔲 SharpZipLib (Archiving to .7z .zip)
- 🔲 Generate Excel File
- 🔲 Use Polly for retries
