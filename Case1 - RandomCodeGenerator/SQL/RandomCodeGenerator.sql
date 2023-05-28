CREATE PROCEDURE generate_codes
AS
BEGIN
    DECLARE @Codes TABLE (Code NVARCHAR(8));
    DECLARE @NumberOfCodes INT = 1000;
	DECLARE @AllowedCharacters NVARCHAR(50) = 'ACDEFGHKLMNPRTXYZ234579';

    WHILE (SELECT COUNT(*) FROM @Codes) < @NumberOfCodes
		BEGIN
			DECLARE @Code NVARCHAR(8) = '';
			
			WHILE LEN(@Code) < 8
				BEGIN
					DECLARE @RandomIndex INT = CEILING(RAND() * LEN(@AllowedCharacters));
					SET @Code += SUBSTRING(@AllowedCharacters, @RandomIndex, 1);
				END

			IF NOT EXISTS (SELECT 1 FROM @Codes WHERE Code = @Code)
				BEGIN
					DECLARE @IsValid INT;
					EXEC [dbo].[check_code] @Code, @IsValid OUTPUT;

					IF @IsValid = 1
						BEGIN
							INSERT INTO @Codes (Code) VALUES (@code);
						END
				END
		END

    SELECT Code FROM @Codes;
END
GO


CREATE PROCEDURE check_code
    @Code VARCHAR(8),
    @IsValid INT OUTPUT
AS
BEGIN
    SET @IsValid = 0; 

    IF LEN(@Code) = 8 AND @Code NOT LIKE '%[^ACDEFGHKLMNPRTXYZ234579]%'
    BEGIN
        SET @IsValid = 1;
    END;
END;