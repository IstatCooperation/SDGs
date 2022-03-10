use [PxSDGs]

DECLARE @viewName varchar(500)
DECLARE cur CURSOR
      FOR SELECT [name] FROM sys.objects WHERE type = 'v' and [name] like 'S%'
      OPEN cur

      FETCH NEXT FROM cur INTO @viewName
      WHILE @@fetch_status = 0
      BEGIN
            EXEC('DROP VIEW ' + @viewName)
            FETCH NEXT FROM cur INTO @viewName
      END
      CLOSE cur
      DEALLOCATE cur