**How to find where PA task requirements are implemented**

**B-**

1. 
    - Inheritance: SemesterSync/ViewModelLibrary/ANY_VIEWMODEL. All viewmodels inherit from INotifyPropertyChanged
    - Polymorphism: SemesterSync/ViewModelLibrary/NotificationPopupViewModel. The OnAppearing method can take a Class or DetailedExam as a parameter and display information accordingly.
    - Encapsulation: SemesterSync/ViewModelLibrary/ANY_VIEWMODEL. All viewmodels show encapsulation by having private fields that are accessed with public properties.

2. Search Functionality: On All Terms page, users can search by class name or term name. On All Classes page, users can search by class name.

3. Database Component: SemesterSync features a SQLite database that is only accessible from the users device. SemesterSync also supports multiple users on the same device by only allowing queries for the active user.

4. Reports: Users can generate reports that divide their classes by status. Users can view a report for all terms or select a specif turn.

5. Validation: All user inputs validate that the input data is present and correct.

6. Security Features: SemesterSync includes a login page allowing users to only access the data they have created. All passwords are salted and hashed before being stored in the database.

7. Scalable Design: All lists are contained in a ScrollView, ListView, or CollectionView, allowing unlimited rows of data to be added without breaking the user interface.

8. Functional GUI: SemesterSync offers an easy to use GUI that allows multiple, logical routes to access the desired page.