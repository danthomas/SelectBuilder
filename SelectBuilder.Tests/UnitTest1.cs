using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SelectBuilder.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Count()
        {
            SelectStatement selectStatement = CreateBuilder().CreateSelect("inventory.Warrant", "w")
                .Select("AccountId")
                .Select("WarrantId", "NoWarrants", aggregate: Aggregates.Count)
                .Where("AccountId", Operator.IsNotNull, null, null);

            Assert.AreEqual(@"
SELECT      w.AccountId
          , COUNT(w.WarrantId) NoWarrants
FROM        inventory.Warrant w
WHERE       w.AccountId IS NOT NULL
GROUP BY    w.AccountId", selectStatement.Statement);
        }

        [TestMethod]
        public void SubQuery()
        {
            Builder builder = CreateBuilder();

            SelectStatement sub = builder.CreateSelect("inventory.Warrant", "w")
                .Select("AccountId")
                .Select("WarrantId", "NoWarrants", aggregate: Aggregates.Count)
                .Where("AccountId", Operator.IsNotNull, null, null);

            SelectStatement selectStatement = builder.CreateSelect("companies.Account", "a", isPaged: true)
                .Join("a.AccountTypeCode", "at")
                .Join("a.CompanyId", "c")
                .Join("a.AccountId", sub, "x", "AccountId")
                .Select("a.AccountId", columnType: ColumnType.Identifier)
                .Select("a.AccountCode")
                .Select("a.AccountName")
                .Select("at.AccountTypeName")
                .Select("c.CompanyCode")
                .Select("c.CompanyName")
                .Select("a.IsActive")
                .Select("x.NoWarrants");

            Assert.AreEqual(@"
SELECT      a.AccountId
          , a.AccountCode
          , a.AccountName
          , at.AccountTypeName
          , c.CompanyCode
          , c.CompanyName
          , a.IsActive
          , x.NoWarrants
          , COUNT(*) OVER(PARTITION BY '') TotalRecords
FROM        companies.Account a
JOIN        companies.AccountType at ON a.AccountTypeCode = at.AccountTypeCode
JOIN        companies.Company c ON a.CompanyId = c.CompanyId
LEFT JOIN   (            
            SELECT      w.AccountId
                      , COUNT(w.WarrantId) NoWarrants
            FROM        inventory.Warrant w
            WHERE       w.AccountId IS NOT NULL
            GROUP BY    w.AccountId
            ) x ON a.AccountId = x.AccountId
ORDER BY    a.AccountId
OFFSET 0 ROWS FETCH NEXT 20 ROWS ONLY", selectStatement.Statement);
        }

        [TestMethod]
        public void SingleTable()
        {
            SelectStatement selectStatement = CreateBuilder().CreateSelect("core.Config", "c")
                .Select("c.ConfigId")
                .Select("c.ConfigName")
                .Select("c.ConfigValue");

            Assert.AreEqual(@"
SELECT      c.ConfigId
          , c.ConfigName
          , c.ConfigValue
FROM        core.Config c", selectStatement.Statement);
        }

        [TestMethod]
        public void TwoTables_ForeignToPrimary()
        {
            SelectStatement selectStatement = CreateBuilder().CreateSelect("companies.Company", "c")
                .Join("c.CompanyTypeId", "ct")
                .Select("c.CompanyId")
                .Select("c.CompanyCode")
                .Select("c.CompanyName")
                .Select("ct.CompanyTypeName");

            Assert.AreEqual(@"
SELECT      c.CompanyId
          , c.CompanyCode
          , c.CompanyName
          , ct.CompanyTypeName
FROM        companies.Company c
JOIN        companies.CompanyType ct ON c.CompanyTypeId = ct.CompanyTypeId", selectStatement.Statement);
        }

        [TestMethod]
        public void TwoTables_PrimaryToForeign()
        {
            SelectStatement selectStatement = CreateBuilder().CreateSelect("core.ProductType", "pt")
                .Join("pt", "core.Product", "p")
                .Select("p.ProductId")
                .Select("pt.ProductTypeCode")
                .Select("p.ProductCode");

            Assert.AreEqual(@"
SELECT      p.ProductId
          , pt.ProductTypeCode
          , p.ProductCode
FROM        core.ProductType pt
JOIN        core.Product p ON pt.ProductTypeId = p.ProductTypeId", selectStatement.Statement);
        }

        [TestMethod]
        public void SeveralTables()
        {
            SelectStatement selectStatement = CreateBuilder().CreateSelect("inventory.Request", "r")

                .Join("r.RequestTypeId", "rt")
                .Join("r.FromCompanyId", "fc")
                .Join("r.ToCompanyId", "tc")
                .Join("r", "inventory.RequestWarrant", "rw")
                .Join("rw.WarrantId", "w")
                .Join("w.ContractId", "c")
                .Join("c.ProductId", "p")
                .Join("p.ProductTypeId", "pt")

                .Select("RequestWarrantId")
                .Select("RequestId")
                .Select("RequestTypeCode")
                .Select("fc.CompanyCode", alias: "From")
                .Select("tc.CompanyCode", alias: "To")
                .Select("WarrantNumber")
                .Select("ProductCode")
                .Select("ProductTypeCode");

            Assert.AreEqual(@"
SELECT      rw.RequestWarrantId
          , r.RequestId
          , rt.RequestTypeCode
          , fc.CompanyCode [From]
          , tc.CompanyCode [To]
          , w.WarrantNumber
          , p.ProductCode
          , pt.ProductTypeCode
FROM        inventory.Request r
JOIN        inventory.RequestType rt ON r.RequestTypeId = rt.RequestTypeId
JOIN        companies.Company fc ON r.FromCompanyId = fc.CompanyId
JOIN        companies.Company tc ON r.ToCompanyId = tc.CompanyId
JOIN        inventory.RequestWarrant rw ON r.RequestId = rw.RequestId
JOIN        inventory.Warrant w ON rw.WarrantId = w.WarrantId
JOIN        core.Contract c ON w.ContractId = c.ContractId
JOIN        core.Product p ON c.ProductId = p.ProductId
JOIN        core.ProductType pt ON p.ProductTypeId = pt.ProductTypeId", selectStatement.Statement);
        }

        [TestMethod]
        public void SeveralTables_InvisibleColumns()
        {
            SelectStatement selectStatement = CreateBuilder().CreateSelect("inventory.Request", "r")
                .Join("r.RequestTypeId", "rt")
                .Join("r.FromCompanyId", "fc")
                .Join("r.ToCompanyId", "tc")
                .Join("r", "inventory.RequestWarrant", "rw")
                .Join("rw.WarrantId", "w")
                .Join("w.ContractId", "c")
                .Join("c.ProductId", "p")
                .Join("p.ProductTypeId", "pt")

                .Select("RequestWarrantId")
                .Select("RequestId", isVisible: false)
                .Select("RequestTypeCode", isVisible: false)
                .Select("fc.CompanyCode", alias: "From", isVisible: false)
                .Select("tc.CompanyCode", alias: "To", isVisible: false)
                .Select("WarrantNumber")
                .Select("ProductCode", isVisible: false)
                .Select("ProductTypeCode", isVisible: false);

            Assert.AreEqual(@"
SELECT      rw.RequestWarrantId
          , w.WarrantNumber
FROM        inventory.Request r
JOIN        inventory.RequestWarrant rw ON r.RequestId = rw.RequestId
JOIN        inventory.Warrant w ON rw.WarrantId = w.WarrantId", selectStatement.Statement);
        }

        [TestMethod]
        public void SingleTableWithSingleWhere()
        {
            SelectStatement selectStatement = CreateBuilder().CreateSelect("core.Config", "c")
                .Select("c.ConfigId")
                .Select("c.ConfigName")
                .Select("c.ConfigValue")
                .Where("c.ConfigName", Operator.StartsWith, "X", null);

            Assert.AreEqual(@"
SELECT      c.ConfigId
          , c.ConfigName
          , c.ConfigValue
FROM        core.Config c
WHERE       c.ConfigName LIKE @p1 + '%'", selectStatement.Statement);
        }

        [TestMethod]
        public void BitWhere()
        {
            SelectStatement selectStatement = CreateBuilder().CreateSelect("core.Config", "c")
                .Select("c.ConfigId")
                .Select("c.ConfigName")
                .Select("c.ConfigValue")
                .Select("c.IsActive")
                .Where("c.IsActive", Operator.IsTrue, null, null);

            Assert.AreEqual(@"
SELECT      c.ConfigId
          , c.ConfigName
          , c.ConfigValue
          , c.IsActive
FROM        core.Config c
WHERE       c.IsActive = 1", selectStatement.Statement);
        }

        [TestMethod]
        public void SingleTableWithTwoWheres()
        {
            SelectStatement selectStatement = CreateBuilder().CreateSelect("core.Config", "c")
                .Select("c.ConfigId")
                .Select("c.ConfigName")
                .Select("c.ConfigValue")
                .Where("c.ConfigName", Operator.StartsWith, "X", null)
                .Where("c.ConfigValue", Operator.Contains, "Y", null);

            Assert.AreEqual(@"
SELECT      c.ConfigId
          , c.ConfigName
          , c.ConfigValue
FROM        core.Config c
WHERE       c.ConfigName LIKE @p1 + '%'
AND         c.ConfigValue LIKE '%' + @p2 + '%'", selectStatement.Statement);
        }

        [TestMethod]
        public void SeveralTablesWithWhereCondition()
        {
            SelectStatement selectStatement = CreateBuilder().CreateSelect("inventory.Request", "r")

                .Join("r.RequestTypeId", "rt")
                .Join("r.FromCompanyId", "fc")
                .Join("r.ToCompanyId", "tc")
                .Join("r", "inventory.RequestWarrant", "rw")
                .Join("rw.WarrantId", "w")
                .Join("w.ContractId", "c")
                .Join("c.ProductId", "p")
                .Join("p.ProductTypeId", "pt")

                .Select("RequestWarrantId")
                .Select("RequestId")
                .Select("WarrantNumber")

                .Where("ProductTypeCode", Operator.Contains, "a", null);

            Assert.AreEqual(@"
SELECT      rw.RequestWarrantId
          , r.RequestId
          , w.WarrantNumber
FROM        inventory.Request r
JOIN        inventory.RequestWarrant rw ON r.RequestId = rw.RequestId
JOIN        inventory.Warrant w ON rw.WarrantId = w.WarrantId
JOIN        core.Contract c ON w.ContractId = c.ContractId
JOIN        core.Product p ON c.ProductId = p.ProductId
JOIN        core.ProductType pt ON p.ProductTypeId = pt.ProductTypeId
WHERE       pt.ProductTypeCode LIKE '%' + @p1 + '%'", selectStatement.Statement);
        }

        [TestMethod]
        public void SelectWithOrderBy()
        {
            SelectStatement selectStatement = CreateBuilder().CreateSelect("core.Config", "c")
                .Select("c.ConfigId")
                .Select("c.ConfigName", sortOrder: 2)
                .Select("c.ConfigValue", sortOrder: -1);

            Assert.AreEqual(@"
SELECT      c.ConfigId
          , c.ConfigName
          , c.ConfigValue
FROM        core.Config c
ORDER BY    c.ConfigValue DESC
          , c.ConfigName", selectStatement.Statement);
        }

        [TestMethod]
        public void OrderBy()
        {
            SelectStatement selectStatement = CreateBuilder().CreateSelect("core.Config", "c")
                .Select("c.ConfigId")
                .Select("c.ConfigName")
                .Select("c.ConfigValue")
                .OrderBy("ConfigName");

            Assert.AreEqual(@"
SELECT      c.ConfigId
          , c.ConfigName
          , c.ConfigValue
FROM        core.Config c
ORDER BY    c.ConfigName", selectStatement.Statement);
        }

        [TestMethod]
        public void OrderByDesc()
        {
            SelectStatement selectStatement = CreateBuilder().CreateSelect("core.Config", "c")
                .Select("c.ConfigId")
                .Select("c.ConfigName")
                .Select("c.ConfigValue")
                .OrderBy("ConfigName")
                .OrderBy("ConfigName");

            Assert.AreEqual(@"
SELECT      c.ConfigId
          , c.ConfigName
          , c.ConfigValue
FROM        core.Config c
ORDER BY    c.ConfigName DESC", selectStatement.Statement);
        }

        [TestMethod]
        public void OrderBySeveral()
        {
            SelectStatement selectStatement = CreateBuilder().CreateSelect("core.Config", "c")
                .Select("c.ConfigId")
                .Select("c.ConfigName")
                .Select("c.ConfigValue")
                .OrderBy("ConfigName")
                .OrderBy("ConfigName")
                .OrderBy("ConfigValue");

            Assert.AreEqual(@"
SELECT      c.ConfigId
          , c.ConfigName
          , c.ConfigValue
FROM        core.Config c
ORDER BY    c.ConfigValue
          , c.ConfigName DESC", selectStatement.Statement);
        }

        private static Builder CreateBuilder()
        {
            TestDataSource testDataSource = CreateTestDataSource();

            Builder builder = new Builder(testDataSource);

            builder.LoadDefinitions();

            return builder;
        }

        private static TestDataSource CreateTestDataSource()
        {
            TestDataSource testDataSource = new TestDataSource();

            testDataSource.AddSchema("dbo");
            testDataSource.AddSchema("admin");
            testDataSource.AddSchema("audit");
            testDataSource.AddSchema("companies");
            testDataSource.AddSchema("core");
            testDataSource.AddSchema("inventory");
            testDataSource.AddSchema("messaging");
            testDataSource.AddSchema("security");
            testDataSource.AddSchema("storage");

            testDataSource.AddObject("admin", "AppVersion");
            testDataSource.AddObject("audit", "AppVersionHistory");
            testDataSource.AddObject("companies", "CompanyType");
            testDataSource.AddObject("companies", "AccountType");
            testDataSource.AddObject("companies", "Company");
            testDataSource.AddObject("companies", "Account");
            testDataSource.AddObject("core", "ErrorLog");
            testDataSource.AddObject("core", "Config");
            testDataSource.AddObject("core", "LookupEntity");
            testDataSource.AddObject("core", "LookupEntityType");
            testDataSource.AddObject("core", "UnitMeasure");
            testDataSource.AddObject("core", "Location");
            testDataSource.AddObject("core", "ProductType");
            testDataSource.AddObject("core", "Product");
            testDataSource.AddObject("core", "Contract");
            testDataSource.AddObject("core", "ContractUnit");
            testDataSource.AddObject("core", "Grid");
            testDataSource.AddObject("core", "BatchItem");
            testDataSource.AddObject("core", "Batch");
            testDataSource.AddObject("core", "Address");
            testDataSource.AddObject("inventory", "Warrant");
            testDataSource.AddObject("inventory", "Unit");
            testDataSource.AddObject("inventory", "RequestStatus");
            testDataSource.AddObject("inventory", "RequestType");
            testDataSource.AddObject("inventory", "RequestWarrant");
            testDataSource.AddObject("inventory", "Request");
            testDataSource.AddObject("inventory", "WarrantMovement");
            testDataSource.AddObject("messaging", "Message");
            testDataSource.AddObject("messaging", "MessageType");
            testDataSource.AddObject("security", "RoleFeature");
            testDataSource.AddObject("security", "Feature");
            testDataSource.AddObject("storage", "StoreProduct");
            testDataSource.AddObject("storage", "Store");
            testDataSource.AddObject("dbo", "DatabaseVersions");
            testDataSource.AddObject("dbo", "UserProfile");
            testDataSource.AddObject("dbo", "webpages_Membership");
            testDataSource.AddObject("dbo", "webpages_Roles");
            testDataSource.AddObject("dbo", "webpages_UsersInRoles");
            testDataSource.AddObject("dbo", "__RefactorLog");
            testDataSource.AddColumn("admin", "AppVersion", "AppVersionId", "tinyint", 1, false, true, null);
            testDataSource.AddColumn("admin", "AppVersion", "VersionNumber", "varchar", 50, false, false, null);
            testDataSource.AddColumn("admin", "AppVersion", "ComponentCode", "varchar", 50, false, false, null);
            testDataSource.AddColumn("admin", "AppVersion", "ComponentName", "varchar", 200, false, false, null);
            testDataSource.AddColumn("admin", "AppVersion", "IsSolution", "bit", 1, false, false, null);
            testDataSource.AddColumn("admin", "AppVersion", "Notes", "varchar", -1, true, false, null);
            testDataSource.AddColumn("admin", "AppVersion", "ServerList", "varchar", -1, true, false, null);
            testDataSource.AddColumn("admin", "AppVersion", "CreateDateTimeUtc", "smalldatetime", 4, false, false, null);
            testDataSource.AddColumn("admin", "AppVersion", "CreateUserName", "varchar", 100, false, false, null);
            testDataSource.AddColumn("admin", "AppVersion", "UpdateDateTimeUtc", "smalldatetime", 4, true, false, null);
            testDataSource.AddColumn("admin", "AppVersion", "UpdateUserName", "varchar", 100, true, false, null);
            testDataSource.AddColumn("audit", "AppVersionHistory", "AppVersionHistoryId", "tinyint", 1, false, true, null);
            testDataSource.AddColumn("audit", "AppVersionHistory", "AppVersionId", "tinyint", 1, false, false, null);
            testDataSource.AddColumn("audit", "AppVersionHistory", "VersionNumber", "varchar", 50, false, false, null);
            testDataSource.AddColumn("audit", "AppVersionHistory", "ComponentCode", "varchar", 50, false, false, null);
            testDataSource.AddColumn("audit", "AppVersionHistory", "ComponentName", "varchar", 200, false, false, null);
            testDataSource.AddColumn("audit", "AppVersionHistory", "IsSolution", "bit", 1, false, false, null);
            testDataSource.AddColumn("audit", "AppVersionHistory", "Notes", "varchar", -1, true, false, null);
            testDataSource.AddColumn("audit", "AppVersionHistory", "ServerList", "varchar", -1, true, false, null);
            testDataSource.AddColumn("audit", "AppVersionHistory", "CreateDateTimeUtc", "smalldatetime", 4, false, false, null);
            testDataSource.AddColumn("audit", "AppVersionHistory", "CreateUserName", "varchar", 100, false, false, null);
            testDataSource.AddColumn("audit", "AppVersionHistory", "UpdateDateTimeUtc", "smalldatetime", 4, true, false, null);
            testDataSource.AddColumn("audit", "AppVersionHistory", "UpdateUserName", "varchar", 100, true, false, null);
            testDataSource.AddColumn("audit", "AppVersionHistory", "VersionHistoryDateTimeUtc", "varchar", 100, false, false, null);
            testDataSource.AddColumn("audit", "AppVersionHistory", "Action", "char", 1, false, false, null);
            testDataSource.AddColumn("companies", "CompanyType", "CompanyTypeId", "tinyint", 1, false, true, null);
            testDataSource.AddColumn("companies", "CompanyType", "CompanyTypeCode", "varchar", 50, false, false, null);
            testDataSource.AddColumn("companies", "CompanyType", "CompanyTypeName", "varchar", 50, false, false, null);
            testDataSource.AddColumn("companies", "CompanyType", "CanSeeAllData", "bit", 1, false, false, null);
            testDataSource.AddColumn("companies", "AccountType", "AccountTypeCode", "char", 1, false, true, null);
            testDataSource.AddColumn("companies", "AccountType", "AccountTypeName", "varchar", 50, false, false, null);
            testDataSource.AddColumn("companies", "AccountType", "IsActive", "bit", 1, false, false, null);
            testDataSource.AddColumn("companies", "Company", "CompanyId", "smallint", 2, false, true, null);
            testDataSource.AddColumn("companies", "Company", "CompanyCode", "varchar", 5, false, false, null);
            testDataSource.AddColumn("companies", "Company", "CompanyName", "varchar", 100, false, false, null);
            testDataSource.AddColumn("companies", "Company", "CompanyTypeId", "tinyint", 1, false, false, "CompanyType");
            testDataSource.AddColumn("companies", "Company", "Address", "varchar", 250, false, false, null);
            testDataSource.AddColumn("companies", "Company", "PostCode", "varchar", 10, false, false, null);
            testDataSource.AddColumn("companies", "Company", "Telephone", "varchar", 20, false, false, null);
            testDataSource.AddColumn("companies", "Company", "IsActive", "bit", 1, false, false, null);
            testDataSource.AddColumn("companies", "Company", "LastModified", "datetime2", 6, false, false, null);
            testDataSource.AddColumn("companies", "Account", "AccountId", "smallint", 2, false, true, null);
            testDataSource.AddColumn("companies", "Account", "AccountCode", "varchar", 20, false, false, null);
            testDataSource.AddColumn("companies", "Account", "AccountName", "varchar", 100, true, false, null);
            testDataSource.AddColumn("companies", "Account", "AccountTypeCode", "char", 1, false, false, "AccountType");
            testDataSource.AddColumn("companies", "Account", "CompanyId", "smallint", 2, false, false, "Company");
            testDataSource.AddColumn("companies", "Account", "IsActive", "bit", 1, false, false, null);
            testDataSource.AddColumn("core", "ErrorLog", "ErrorLogId", "int", 4, false, true, null);
            testDataSource.AddColumn("core", "ErrorLog", "Date", "datetime2", 6, false, false, null);
            testDataSource.AddColumn("core", "ErrorLog", "Thread", "nvarchar", 510, false, false, null);
            testDataSource.AddColumn("core", "ErrorLog", "Level", "nvarchar", 100, false, false, null);
            testDataSource.AddColumn("core", "ErrorLog", "Logger", "nvarchar", 510, false, false, null);
            testDataSource.AddColumn("core", "ErrorLog", "Message", "nvarchar", 8000, false, false, null);
            testDataSource.AddColumn("core", "ErrorLog", "Exception", "nvarchar", -1, true, false, null);
            testDataSource.AddColumn("core", "ErrorLog", "MachineName", "nvarchar", 510, true, false, null);
            testDataSource.AddColumn("core", "ErrorLog", "ApplicationVersion", "varchar", 100, true, false, null);
            testDataSource.AddColumn("core", "ErrorLog", "SupportNumber", "nvarchar", 40, true, false, null);
            testDataSource.AddColumn("core", "ErrorLog", "Username", "nvarchar", 200, true, false, null);
            testDataSource.AddColumn("core", "Config", "ConfigId", "int", 4, false, true, null);
            testDataSource.AddColumn("core", "Config", "ConfigName", "varchar", 28, true, false, null);
            testDataSource.AddColumn("core", "Config", "ConfigValue", "varchar", 255, true, false, null);
            testDataSource.AddColumn("core", "Config", "IsActive", "bit", 1, true, false, null);
            testDataSource.AddColumn("core", "LookupEntity", "LookupEntityId", "int", 4, false, true, null);
            testDataSource.AddColumn("core", "LookupEntity", "LookupEntityTypeId", "int", 4, false, false, "LookupEntityType");
            testDataSource.AddColumn("core", "LookupEntity", "LookupEntityDesc", "nvarchar", 100, true, false, null);
            testDataSource.AddColumn("core", "LookupEntity", "LookupEntityCode", "varchar", 10, false, false, null);
            testDataSource.AddColumn("core", "LookupEntity", "IsActive", "bit", 1, false, false, null);
            testDataSource.AddColumn("core", "LookupEntityType", "LookupEntityTypeId", "int", 4, false, true, null);
            testDataSource.AddColumn("core", "LookupEntityType", "LookupEntityTypeDesc", "nvarchar", 100, true, false, null);
            testDataSource.AddColumn("core", "LookupEntityType", "LookupEntityTypeCode", "varchar", 10, false, false, null);
            testDataSource.AddColumn("core", "UnitMeasure", "UnitMeasureId", "tinyint", 1, false, true, null);
            testDataSource.AddColumn("core", "UnitMeasure", "UnitMeasureCode", "varchar", 20, false, false, null);
            testDataSource.AddColumn("core", "UnitMeasure", "UnitMeasureName", "varchar", 100, false, false, null);
            testDataSource.AddColumn("core", "UnitMeasure", "ConversionToBaseMeasure", "decimal", 9, false, false, null);
            testDataSource.AddColumn("core", "UnitMeasure", "IsActive", "bit", 1, false, false, null);
            testDataSource.AddColumn("core", "Location", "LocationId", "smallint", 2, false, true, null);
            testDataSource.AddColumn("core", "Location", "LocationCode", "varchar", 20, false, false, null);
            testDataSource.AddColumn("core", "Location", "LocationName", "varchar", 100, false, false, null);
            testDataSource.AddColumn("core", "Location", "IsActive", "bit", 1, false, false, null);
            testDataSource.AddColumn("core", "ProductType", "ProductTypeId", "tinyint", 1, false, true, null);
            testDataSource.AddColumn("core", "ProductType", "ProductTypeCode", "varchar", 50, false, false, null);
            testDataSource.AddColumn("core", "ProductType", "ProductTypeName", "varchar", 100, false, false, null);
            testDataSource.AddColumn("core", "Product", "ProductId", "tinyint", 1, false, true, null);
            testDataSource.AddColumn("core", "Product", "ProductCode", "varchar", 20, false, false, null);
            testDataSource.AddColumn("core", "Product", "ProductName", "varchar", 100, false, false, null);
            testDataSource.AddColumn("core", "Product", "ProductTypeId", "tinyint", 1, false, false, "ProductType");
            testDataSource.AddColumn("core", "Product", "UnitMeasureId", "tinyint", 1, false, false, "UnitMeasure");
            testDataSource.AddColumn("core", "Product", "SizeMaxDp", "tinyint", 1, false, false, null);
            testDataSource.AddColumn("core", "Product", "AutoIssueWarrants", "bit", 1, false, false, null);
            testDataSource.AddColumn("core", "Product", "IsActive", "bit", 1, false, false, null);
            testDataSource.AddColumn("core", "Contract", "ContractId", "tinyint", 1, false, true, null);
            testDataSource.AddColumn("core", "Contract", "ContractCode", "varchar", 20, false, false, null);
            testDataSource.AddColumn("core", "Contract", "ContractName", "varchar", 100, false, false, null);
            testDataSource.AddColumn("core", "Contract", "ExchangeCompanyId", "smallint", 2, false, false, "Company");
            testDataSource.AddColumn("core", "Contract", "ProductId", "tinyint", 1, false, false, "Product");
            testDataSource.AddColumn("core", "Contract", "IsActive", "bit", 1, false, false, null);
            testDataSource.AddColumn("core", "ContractUnit", "ContractUnitId", "tinyint", 1, false, true, null);
            testDataSource.AddColumn("core", "ContractUnit", "ContractId", "tinyint", 1, false, false, "Contract");
            testDataSource.AddColumn("core", "ContractUnit", "ContractUnitCode", "varchar", 10, false, false, null);
            testDataSource.AddColumn("core", "ContractUnit", "NetSize", "decimal", 9, false, false, null);
            testDataSource.AddColumn("core", "ContractUnit", "UnitMeasureId", "tinyint", 1, false, false, "UnitMeasure");
            testDataSource.AddColumn("core", "ContractUnit", "TolerancePercentage", "decimal", 5, false, false, null);
            testDataSource.AddColumn("core", "Grid", "GridId", "tinyint", 1, false, true, null);
            testDataSource.AddColumn("core", "Grid", "GridName", "varchar", 50, false, false, null);
            testDataSource.AddColumn("core", "BatchItem", "BatchId", "int", 4, false, true, "Batch");
            testDataSource.AddColumn("core", "BatchItem", "ItemId", "int", 4, false, true, null);
            testDataSource.AddColumn("core", "Batch", "BatchId", "int", 4, false, true, null);
            testDataSource.AddColumn("core", "Batch", "GridId", "tinyint", 1, false, false, "Grid");
            testDataSource.AddColumn("core", "Batch", "UserId", "int", 4, false, false, "UserProfile");
            testDataSource.AddColumn("core", "Batch", "BatchName", "varchar", 50, false, false, null);
            testDataSource.AddColumn("core", "Address", "AddressId", "smallint", 2, false, true, null);
            testDataSource.AddColumn("core", "Address", "Address1", "varchar", 100, false, false, null);
            testDataSource.AddColumn("core", "Address", "Address2", "varchar", 100, true, false, null);
            testDataSource.AddColumn("core", "Address", "Address3", "varchar", 100, true, false, null);
            testDataSource.AddColumn("core", "Address", "Address4", "varchar", 100, true, false, null);
            testDataSource.AddColumn("core", "Address", "PostCode", "varchar", 20, false, false, null);
            testDataSource.AddColumn("core", "Address", "Country", "varchar", 30, false, false, null);
            testDataSource.AddColumn("core", "Address", "Telephone", "varchar", 30, false, false, null);
            testDataSource.AddColumn("inventory", "Warrant", "WarrantId", "int", 4, false, true, null);
            testDataSource.AddColumn("inventory", "Warrant", "WarrantNumber", "varchar", 30, false, false, null);
            testDataSource.AddColumn("inventory", "Warrant", "StorageCompanyId", "smallint", 2, false, false, "Company");
            testDataSource.AddColumn("inventory", "Warrant", "StoreId", "smallint", 2, false, false, "Store");
            testDataSource.AddColumn("inventory", "Warrant", "LocationId", "smallint", 2, false, false, "Location");
            testDataSource.AddColumn("inventory", "Warrant", "ContractId", "tinyint", 1, false, false, "Contract");
            testDataSource.AddColumn("inventory", "Warrant", "ContractUnitId", "tinyint", 1, false, false, "ContractUnit");
            testDataSource.AddColumn("inventory", "Warrant", "DateIssued", "date", 3, false, false, null);
            testDataSource.AddColumn("inventory", "Warrant", "NetSize", "decimal", 9, false, false, null);
            testDataSource.AddColumn("inventory", "Warrant", "GrossSize", "decimal", 9, false, false, null);
            testDataSource.AddColumn("inventory", "Warrant", "UnitMeasureId", "tinyint", 1, false, false, "UnitMeasure");
            testDataSource.AddColumn("inventory", "Warrant", "AssignedToCompanyId", "smallint", 2, true, false, "Company");
            testDataSource.AddColumn("inventory", "Warrant", "AccountId", "smallint", 2, true, false, "Account");
            testDataSource.AddColumn("inventory", "Unit", "UnitId", "int", 4, false, true, null);
            testDataSource.AddColumn("inventory", "Unit", "UnitNumber", "varchar", 30, false, false, null);
            testDataSource.AddColumn("inventory", "Unit", "StoreId", "smallint", 2, false, false, "Store");
            testDataSource.AddColumn("inventory", "Unit", "StorageCompanyId", "smallint", 2, false, false, "Company");
            testDataSource.AddColumn("inventory", "Unit", "LocationId", "smallint", 2, false, false, "Location");
            testDataSource.AddColumn("inventory", "Unit", "ProductId", "tinyint", 1, false, false, "Product");
            testDataSource.AddColumn("inventory", "Unit", "DateStored", "date", 3, true, false, null);
            testDataSource.AddColumn("inventory", "Unit", "NetSize", "decimal", 9, true, false, null);
            testDataSource.AddColumn("inventory", "Unit", "GrossSize", "decimal", 9, true, false, null);
            testDataSource.AddColumn("inventory", "Unit", "UnitMeasureId", "tinyint", 1, true, false, "UnitMeasure");
            testDataSource.AddColumn("inventory", "Unit", "Owner", "varchar", 50, false, false, null);
            testDataSource.AddColumn("inventory", "Unit", "Reference1", "varchar", 50, false, false, null);
            testDataSource.AddColumn("inventory", "Unit", "Reference2", "varchar", 50, false, false, null);
            testDataSource.AddColumn("inventory", "Unit", "Reference3", "varchar", 50, false, false, null);
            testDataSource.AddColumn("inventory", "Unit", "Notes1", "varchar", 1000, false, false, null);
            testDataSource.AddColumn("inventory", "Unit", "Notes2", "varchar", 1000, false, false, null);
            testDataSource.AddColumn("inventory", "Unit", "Notes3", "varchar", 1000, false, false, null);
            testDataSource.AddColumn("inventory", "Unit", "WarrantId", "int", 4, true, false, "Warrant");
            testDataSource.AddColumn("inventory", "RequestStatus", "RequestStatusId", "tinyint", 1, false, true, null);
            testDataSource.AddColumn("inventory", "RequestStatus", "RequestStatusCode", "varchar", 20, false, false, null);
            testDataSource.AddColumn("inventory", "RequestStatus", "RequestStatusName", "varchar", 30, false, false, null);
            testDataSource.AddColumn("inventory", "RequestStatus", "RequestTypeId", "tinyint", 1, false, false, "RequestType");
            testDataSource.AddColumn("inventory", "RequestStatus", "IsAccepted", "bit", 1, true, false, null);
            testDataSource.AddColumn("inventory", "RequestType", "RequestTypeId", "tinyint", 1, false, true, null);
            testDataSource.AddColumn("inventory", "RequestType", "RequestTypeCode", "varchar", 50, false, false, null);
            testDataSource.AddColumn("inventory", "RequestType", "RequestTypeName", "varchar", 100, false, false, null);
            testDataSource.AddColumn("inventory", "RequestWarrant", "RequestWarrantId", "int", 4, false, true, null);
            testDataSource.AddColumn("inventory", "RequestWarrant", "RequestId", "int", 4, false, false, "Request");
            testDataSource.AddColumn("inventory", "RequestWarrant", "WarrantId", "int", 4, false, false, "Warrant");
            testDataSource.AddColumn("inventory", "RequestWarrant", "RequestStatusId", "tinyint", 1, false, false, "RequestStatus");
            testDataSource.AddColumn("inventory", "RequestWarrant", "IsAccepted", "bit", 1, true, false, null);
            testDataSource.AddColumn("inventory", "Request", "RequestId", "int", 4, false, true, null);
            testDataSource.AddColumn("inventory", "Request", "RequestTypeId", "tinyint", 1, false, false, "RequestType");
            testDataSource.AddColumn("inventory", "Request", "CreatedByCompanyId", "smallint", 2, false, false, "Company");
            testDataSource.AddColumn("inventory", "Request", "FromCompanyId", "smallint", 2, false, false, "Company");
            testDataSource.AddColumn("inventory", "Request", "ToCompanyId", "smallint", 2, false, false, "Company");
            testDataSource.AddColumn("inventory", "Request", "Total", "smallint", 2, false, false, null);
            testDataSource.AddColumn("inventory", "Request", "IncompleteCount", "smallint", 2, false, false, null);
            testDataSource.AddColumn("inventory", "Request", "AcceptedCount", "smallint", 2, false, false, null);
            testDataSource.AddColumn("inventory", "Request", "RejectedCount", "smallint", 2, false, false, null);
            testDataSource.AddColumn("inventory", "Request", "DateTimeCreated", "datetime2", 6, false, false, null);
            testDataSource.AddColumn("inventory", "WarrantMovement", "WarrantMovementId", "int", 4, false, true, null);
            testDataSource.AddColumn("inventory", "WarrantMovement", "WarrantId", "int", 4, false, false, "Warrant");
            testDataSource.AddColumn("inventory", "WarrantMovement", "AccountHolderCompanyId", "smallint", 2, false, false, "Company");
            testDataSource.AddColumn("inventory", "WarrantMovement", "AccountId", "smallint", 2, true, false, "Account");
            testDataSource.AddColumn("inventory", "WarrantMovement", "MovementDateTimeUtc", "datetime2", 6, false, false, null);
            testDataSource.AddColumn("inventory", "WarrantMovement", "MovedByUserId", "int", 4, false, false, null);
            testDataSource.AddColumn("inventory", "WarrantMovement", "RequestId", "int", 4, true, false, null);
            testDataSource.AddColumn("messaging", "Message", "MessageId", "int", 4, false, true, null);
            testDataSource.AddColumn("messaging", "Message", "MessageTypeId", "tinyint", 1, false, false, "MessageType");
            testDataSource.AddColumn("messaging", "Message", "FromCompanyId", "smallint", 2, false, false, "Company");
            testDataSource.AddColumn("messaging", "Message", "ToCompanyId", "smallint", 2, false, false, "Company");
            testDataSource.AddColumn("messaging", "Message", "Title", "varchar", 100, false, false, null);
            testDataSource.AddColumn("messaging", "Message", "Body", "varchar", -1, false, false, null);
            testDataSource.AddColumn("messaging", "Message", "Sent", "datetime2", 6, false, false, null);
            testDataSource.AddColumn("messaging", "Message", "RequestId", "int", 4, true, false, null);
            testDataSource.AddColumn("messaging", "MessageType", "MessageTypeId", "tinyint", 1, false, true, null);
            testDataSource.AddColumn("messaging", "MessageType", "MessageTypeCode", "varchar", 50, false, false, null);
            testDataSource.AddColumn("messaging", "MessageType", "MessageTypeName", "varchar", 100, false, false, null);
            testDataSource.AddColumn("security", "RoleFeature", "RoleId", "smallint", 2, false, true, "webpages_Roles");
            testDataSource.AddColumn("security", "RoleFeature", "FeatureId", "smallint", 2, false, true, "Feature");
            testDataSource.AddColumn("security", "Feature", "FeatureId", "smallint", 2, false, true, null);
            testDataSource.AddColumn("security", "Feature", "FeatureCode", "varchar", 20, false, false, null);
            testDataSource.AddColumn("security", "Feature", "FeatureName", "nvarchar", 100, false, false, null);
            testDataSource.AddColumn("storage", "StoreProduct", "StoreId", "smallint", 2, false, true, "Store");
            testDataSource.AddColumn("storage", "StoreProduct", "ProductId", "tinyint", 1, false, true, "Product");
            testDataSource.AddColumn("storage", "Store", "StoreId", "smallint", 2, false, true, null);
            testDataSource.AddColumn("storage", "Store", "StorageCompanyId", "smallint", 2, false, false, "Company");
            testDataSource.AddColumn("storage", "Store", "StoreCode", "nvarchar", 100, false, false, null);
            testDataSource.AddColumn("storage", "Store", "StoreName", "nvarchar", 200, false, false, null);
            testDataSource.AddColumn("storage", "Store", "LocationId", "smallint", 2, false, false, "Location");
            testDataSource.AddColumn("storage", "Store", "IsActive", "bit", 1, false, false, null);
            testDataSource.AddColumn("dbo", "DatabaseVersions", "DatabaseVersionId", "int", 4, false, false, null);
            testDataSource.AddColumn("dbo", "DatabaseVersions", "VersionNumber", "varchar", 20, true, false, null);
            testDataSource.AddColumn("dbo", "UserProfile", "UserId", "int", 4, false, true, null);
            testDataSource.AddColumn("dbo", "UserProfile", "CompanyId", "smallint", 2, false, false, "Company");
            testDataSource.AddColumn("dbo", "UserProfile", "UserName", "nvarchar", 60, false, false, null);
            testDataSource.AddColumn("dbo", "UserProfile", "EmailAddress", "nvarchar", 510, false, false, null);
            testDataSource.AddColumn("dbo", "UserProfile", "FirstName", "nvarchar", 510, false, false, null);
            testDataSource.AddColumn("dbo", "UserProfile", "LastName", "nvarchar", 510, false, false, null);
            testDataSource.AddColumn("dbo", "UserProfile", "MemorableWord", "nvarchar", 100, true, false, null);
            testDataSource.AddColumn("dbo", "UserProfile", "MemorableWordIndices", "nvarchar", 22, true, false, null);
            testDataSource.AddColumn("dbo", "UserProfile", "ActiveStatus", "bit", 1, false, false, null);
            testDataSource.AddColumn("dbo", "UserProfile", "IsConfirmed", "bit", 1, false, false, null);
            testDataSource.AddColumn("dbo", "UserProfile", "IsWindowsUser", "bit", 1, false, false, null);
            testDataSource.AddColumn("dbo", "UserProfile", "LastModified", "datetime", 8, false, false, null);
            testDataSource.AddColumn("dbo", "webpages_Membership", "UserId", "int", 4, false, true, null);
            testDataSource.AddColumn("dbo", "webpages_Membership", "CreateDate", "datetime", 8, true, false, null);
            testDataSource.AddColumn("dbo", "webpages_Membership", "ConfirmationToken", "nvarchar", 256, true, false, null);
            testDataSource.AddColumn("dbo", "webpages_Membership", "IsConfirmed", "bit", 1, true, false, null);
            testDataSource.AddColumn("dbo", "webpages_Membership", "LastPasswordFailureDate", "datetime", 8, true, false, null);
            testDataSource.AddColumn("dbo", "webpages_Membership", "PasswordFailuresSinceLastSuccess", "int", 4, false, false, null);
            testDataSource.AddColumn("dbo", "webpages_Membership", "Password", "nvarchar", 256, false, false, null);
            testDataSource.AddColumn("dbo", "webpages_Membership", "PasswordChangedDate", "datetime", 8, true, false, null);
            testDataSource.AddColumn("dbo", "webpages_Membership", "PasswordSalt", "nvarchar", 256, false, false, null);
            testDataSource.AddColumn("dbo", "webpages_Membership", "PasswordVerificationToken", "nvarchar", 256, true, false, null);
            testDataSource.AddColumn("dbo", "webpages_Membership", "PasswordVerificationTokenExpirationDate", "datetime", 8, true, false, null);
            testDataSource.AddColumn("dbo", "webpages_Roles", "RoleId", "smallint", 2, false, true, null);
            testDataSource.AddColumn("dbo", "webpages_Roles", "RoleName", "nvarchar", 512, false, false, null);
            testDataSource.AddColumn("dbo", "webpages_UsersInRoles", "UserId", "int", 4, false, true, "UserProfile");
            testDataSource.AddColumn("dbo", "webpages_UsersInRoles", "RoleId", "smallint", 2, false, true, "webpages_Roles");
            testDataSource.AddColumn("dbo", "__RefactorLog", "OperationKey", "uniqueidentifier", 16, false, true, null);

            return testDataSource;
        }
    }
}
