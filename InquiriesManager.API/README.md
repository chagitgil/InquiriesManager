# InquiriesManager.API

## תיאור הפרויקט
פרויקט זה הוא שירות API לניהול פניות (Inquiries) המבוסס על ASP.NET Core 8 ו־Entity Framework Core עם מסד נתונים SQLite.  
המערכת מאפשרת ביצוע CRUD מלא על פניות ושאילתת דוח חודשי המשווה בין מספר הפניות בחודשים ושנים שונים.

## מבנה המערכת
- Program.cs – הגדרת השירותים, רישום DbContext, הפעלת Swagger, הפעלת CORS והרצת מיגרציות אוטומטית.
- InquiriesDbContext – הגדרת הקשר למסד הנתונים והטבלה Inquiries.
- Inquiry.cs – מחלקת המודל של פנייה (כולל שם, טלפון, אימייל, מזהה מחלקה, תיאור ותאריך יצירה).
- InquiriesRepository – שכבת גישה לנתונים הכוללת פעולות CRUD ושאילתה לדוח חודשי.
- InquiriesController – בקר API עם פעולות CRUD וקריאה לדוח חודשי.
- MonthlyInquiriesReportRow – מחלקת עזר להצגת תוצאות הדוח החודשי.
- Migrations – קבצי מיגרציות ליצירת הטבלאות במסד הנתונים.
- MonthlyInquiriesReport.sql – גרסה של הפרוצדורה המקורית ב־SQL Server, שהומרה בקוד לשאילתת LINQ.

## יתרונות וחסרונות
שימוש ב־SQLite:
- יתרון: קל לשימוש, אינו דורש התקנות חיצוניות, מתאים לדמו ולסביבות פיתוח.
- חסרון: לא מתאים לעומסים גבוהים או ייצור בקנה מידה גדול.

שימוש ב־Entity Framework Core:
- יתרון: עבודה נוחה עם LINQ במקום SQL גולמי, קוד קריא ותחזוקה פשוטה.
- חסרון: פחות יעיל מול מסדי נתונים גדולים לעומת Stored Procedures ואינדקסים ייעודיים.

שימוש ב־Repository:
- יתרון: בידוד הלוגיקה העסקית מהבקר, מאפשר בדיקות יחידה בצורה קלה.
- חסרון: שכבה נוספת בקוד שצריך לתחזק.

## אבטחה
- המערכת פועלת תחת HTTPS.
- מוגדר מנגנון CORS המאפשר גישה מכל דומיין לצורך פיתוח ודמו. בסביבת ייצור יש להגביל לדומיינים ספציפיים.
- נושא האימות וההרשאות לא ממומש בפרויקט זה וניתן להוספה בעתיד (למשל JWT).

## טיפול בשגיאות
- החזרת קוד 404 כאשר פנייה אינה קיימת.
- החזרת קוד 400 כאשר הנתונים אינם תקינים.
- החזרת 201 עבור יצירה מוצלחת ו־204 עבור עדכון או מחיקה מוצלחים.
- ניתן להוסיף מנגנון Global Exception Handling להרחבה.

## מנגנוני קישור
- שימוש ב־Dependency Injection עבור DbContext ו־Repository.
- מאפשר גמישות והחלפה של רכיבים ללא שינוי לוגיקה.

## Cross-Domain
- מוגדר CORS עם מדיניות AllowAll לצורך פיתוח.
- במעבר לייצור יש להגדיר WithOrigins עם הדומיין המותר בלבד.

## הוראות התקנה והרצה
1. שכפול המאגר:
 git clone https://github.com/YourUser/InquiriesManager.API.git
cd InquiriesManager.API
2. התקנת חבילות:
 dotnet restore
3. הרצת המיגרציות ליצירת מסד הנתונים:
 dotnet ef database update
4. הרצת היישום:
 dotnet run

 השירות יעלה בכתובת:
- https://localhost:7085

5. בדיקה באמצעות Swagger:
גישה ל־ https://localhost:7085/swagger

## דוגמאות Endpoints
- GET /api/inquiries – החזרת כל הפניות.
- GET /api/inquiries/{id} – החזרת פנייה לפי מזהה.
- POST /api/inquiries – יצירת פנייה חדשה.
- PUT /api/inquiries/{id} – עדכון פנייה קיימת.
- DELETE /api/inquiries/{id} – מחיקת פנייה.
- GET /api/inquiries/monthly?year=2025&month=8 – דוח חודשי.
