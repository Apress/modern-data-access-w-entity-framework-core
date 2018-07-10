install-package Microsoft.EntityFrameworkCore.SqlServer
install-package Microsoft.EntityFrameworkCore.Tools
Scaffold-DbContext -Connection "Server=.;Database=WWWingsV1;Trusted_Connection=True;MultipleActiveResultSets=True;" -Provider Microsoft.EntityFrameworkCore.SqlServer -force