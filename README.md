# Generic Windows Service
Model project to demonstrate all the "good implementations"


#### To Do List

- ✅ Dependency Injection
- ✅ Serilog Logger + Rolling File
- ✅ Cron tab (similar) scheduling system for processes (NCrontab.Signed)
- ✅ Unit tests (xUnit, Shouldly, NSubstitute, bogus, Verify)
- ✅ Multi-thread processes (manage maximum threads)
- ✅ Manage thread starting of a process, if same process havent finnished in another thread
- ✅ Adjust versionning
- 🔲 Semaphore for multithreaded process execution
- ✅ Add Factory to spawn different processes
- ✅ Add Few generic processes
- 🔲 Add Integration Tests
- 🔲 Possibility to read adhoc lists
- 🔲 Add Notifier abstarction with possibility to configure multiple notifications (email, etc)
- 🔲 Email sending (FluentEmail to send, PaperCut SMPT server to receive emails in DEV, not to send emails to actual users for testing purposes :grimacing:)
- 🔲 SharpZipLib (Archiving to .7z .zip)
- 🔲 Use shouldly for tests assertions https://github.com/shouldly/shouldly
- 🔲 Use Polly for retries
