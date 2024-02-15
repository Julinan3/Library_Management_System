using System.Runtime.CompilerServices;
using System.IO;
using System;
using System.Text.Json;

class Book
{
    //Kitaplar listesindeki elemanların değerleri.
    public string Name { get; set; }
    public string Author { get; set; }
    public string ISBN_Number { get; set; }
    public int Copy_Number { get; set; }
    public int Borrow_Number { get; set; }
    public bool IsBorrowed { get; set; }
    public DateTime Borrow_Date { get; set; }
    public DateTime Due_Date { get; set; }
    public Book(string name, string author, string isbn_number, int copy_number)
    {
        Name = name;
        Author = author;
        ISBN_Number = isbn_number;
        Copy_Number = copy_number;
        Borrow_Number = 0;
        IsBorrowed = false;
        Borrow_Date = DateTime.Today;
        Due_Date = DateTime.Today;
    }
}
class Library
{
    //Book classının listesi.
    private List<Book> Books = new List<Book>();
    //Save dosyasının nerede oluşacağı.
    private string SavePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "Books.txt");
    //Gerektiğinde verileri kaydetmek için fonskiyon.
    public void SaveBooks()
    {
        string JsonFile = JsonSerializer.Serialize(Books);
        File.WriteAllText(SavePath, JsonFile);
    }
    //Gerektiğinde verileri yüklemek için fonskiyon.
    public void LoadBooks()
    {
        if(File.Exists(SavePath))
        {
            string JsonFile = File.ReadAllText(SavePath);
            Books = JsonSerializer.Deserialize<List<Book>>(JsonFile);
        }
        else
        {
            SaveBooks();
        }
    }   
    public void DeleteBooks()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }
    }
    //Konsol ekranını full temizlemek için fonksiyon.
    public void ClearConsole()
    {
        Console.Clear();
        Console.WriteLine("\x1b[3J");
    }
    //Main menüye dönmek için fonksiyon.
    public void BackMainMenu()
    {
        string? Selected_MenuNumber = Console.ReadLine();
        int ConvertedMenuNumber = Convert.ToInt32(Selected_MenuNumber);

        if (ConvertedMenuNumber == 0)
        {
            ClearConsole();

            UserInterface.MainMenu(this);
        }
    }
    //Kitapların listesini çıkaran fonksiyon.
    public void ListBooks()
    {
        LoadBooks();
        ClearConsole();
        //Herhangi bir kitap veritabanında yoksa olmadığını söyler.
        if (Books.Count == 0)
        {
            Console.WriteLine("          ---------------------------------------------------\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("          No books attached to the database !\n          Add from the main menu/Add Book to Database. ");
            Console.ResetColor();
            Console.WriteLine("            0) Return to Main Menu");

            BackMainMenu();
        }
        else//Eğer varsa kitapları listeler.
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("          List of Books:\n");
            Console.ResetColor();
            foreach (var book in Books)
            {
                Console.WriteLine("             Book Title: " + book.Name);
                Console.WriteLine("             Book Author: " + book.Author);
                Console.WriteLine("             Book ISBN Number: " + book.ISBN_Number);
                Console.WriteLine("             Book Copy Number: " + book.Copy_Number);
                Console.WriteLine("             Number of Books Borrowed: " + book.Borrow_Number);
                Console.WriteLine("          ---------------------------------------------------\n");
            }
            Console.WriteLine("            0) Return to Main Menu");

            BackMainMenu();
        }

    }
    //Veri tabanına eklenmiş kitapları arasında arama yapan fonksiyon.
    public void SearchBook()
    {
        LoadBooks();
        ClearConsole();

        Console.WriteLine("          ---------------------------------------------------\n");
        Console.WriteLine("          Enter the title or author of the book you want to search for: ");

        string? BookNameOrAuthor = Console.ReadLine();

        foreach (var book in Books)
        {
            if(book.Name == BookNameOrAuthor || book.Author == BookNameOrAuthor)
            {
                Console.WriteLine("             Book Title: " + book.Name);
                Console.WriteLine("             Book Author: " + book.Author);
                Console.WriteLine("             Book ISBN Number: " + book.ISBN_Number);
                Console.WriteLine("             Book Copy Number: " + book.Copy_Number);
                Console.WriteLine("             Number of Books Borrowed: " + book.Borrow_Number);
                Console.WriteLine("          ---------------------------------------------------\n");
            }
        }
        Console.WriteLine("            0) Return to Main Menu    1) Search Again");

        //0 veya  1 seçilmesi durumunda neler olucağını seçen kısım.
        string? Selected_MenuNumber = Console.ReadLine();
        int ConvertedMenuNumber = Convert.ToInt32(Selected_MenuNumber);

        if(ConvertedMenuNumber == 0)
        {
            ClearConsole();

            UserInterface.MainMenu(this);
        }
        else if(ConvertedMenuNumber == 1)
        {
            SearchBook();
        }
    }
    //Veri tabanına kitap ekleyen fonksiyon.
    public void AddBook()
    {
        LoadBooks();
        ClearConsole();

        Console.WriteLine("          ---------------------------------------------------\n");
        Console.WriteLine("          Name of the book you want to add: ");
        string? Entered_Name = Console.ReadLine();
        Console.WriteLine("          Author of the book you want to add: ");
        string? Entered_Author = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("          ISBN number must consist of numbers.\n          Must be 13 digits.\n          An ISBN number already in the database should not be entered.\n");
        Console.ResetColor();
        Console.WriteLine("          ISBN number of the book you want to add: ");
        string? Entered_ISBN_Number = Console.ReadLine();
        //Girilen ISBN numarasının uygunluğu için uzun kontrol ifleri.
        if (long.TryParse(Entered_ISBN_Number,out long result) == false)
        {
            while(long.TryParse(Entered_ISBN_Number, out long result2) == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("          ISBN number must consist of numbers only");
                Console.ResetColor();
                Console.WriteLine("          Enter the ISBN number of the book you want to add again: ");
                Entered_ISBN_Number = Console.ReadLine();
            }   
        }
        else if(Entered_ISBN_Number.Length < 13 || Entered_ISBN_Number.Length > 13)
        {
            while(Entered_ISBN_Number.Length < 13 || Entered_ISBN_Number.Length > 13)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("          ISBN number must consist of 13 digits");
                Console.ResetColor();
                Console.WriteLine("          Enter the ISBN number of the book you want to add again: ");
                Entered_ISBN_Number = Console.ReadLine();
            }
        }
        else
        {
            foreach (var book in Books)
            {
                if (book.ISBN_Number == Entered_ISBN_Number)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("          The ISBN number of the book you want to add has been used before");
                    Console.ResetColor();
                    Console.WriteLine("          Enter the ISBN number of the book you want to add again: ");
                    Entered_ISBN_Number = Console.ReadLine();
                    if(book.ISBN_Number == Entered_ISBN_Number) 
                    {
                        AddBook();
                    }
                }
            }
        }

        Console.WriteLine("          Number of copies of the book you want to add: ");
        string? Entered_Copy_Number = Console.ReadLine();
        Console.WriteLine("          Book added to the database!");
        Console.WriteLine("          ---------------------------------------------------\n");
        Books.Add(new Book(Entered_Name, Entered_Author, Entered_ISBN_Number, Convert.ToInt32(Entered_Copy_Number)));
        Console.WriteLine("            0) Return to Main Menu");

        SaveBooks();

        BackMainMenu();
    }
    //Kitap ödünç alma fonksiyonu.
    public void BorrowBook()
    {
        ClearConsole();
        LoadBooks();

        Console.WriteLine("          ---------------------------------------------------\n");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("          You can return to the main menu without starting the process by typing 0.");
        Console.ResetColor();
        Console.WriteLine("          Return a borrowed book within 5 days.");
        Console.WriteLine("          Enter the ISBN number of the book you want to borrow: ");
        //ISBN hatırlanmadığı durumda ana menüye dönme fırsatı verir.
        string? Entered_ISBN_Number = Console.ReadLine();
        if(Entered_ISBN_Number == "0")
        {
            UserInterface.MainMenu(this);
        }
        else//ISBN için kontrol ifleri.
        {
            if (long.TryParse(Entered_ISBN_Number, out long result) == false)
            {
                while (long.TryParse(Entered_ISBN_Number, out long result2) == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("          ISBN number must consist of numbers only");
                    Console.ResetColor();
                    Console.WriteLine("          Enter the ISBN number of the book you want to add again: ");
                    Entered_ISBN_Number = Console.ReadLine();
                }
            }
            else if (Entered_ISBN_Number.Length < 13 || Entered_ISBN_Number.Length > 13)
            {
                while (Entered_ISBN_Number.Length < 13 || Entered_ISBN_Number.Length > 13)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("          ISBN number must consist of 13 digits");
                    Console.ResetColor();
                    Console.WriteLine("          Enter the ISBN number of the book you want to add again: ");
                    Entered_ISBN_Number = Console.ReadLine();
                }
            }
            else
            {
                foreach (var book in Books)
                {
                    if (book.ISBN_Number == Entered_ISBN_Number)
                    {
                        if (book.Copy_Number <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("          There are no copies of books to borrow!");
                            Console.ResetColor();
                            Console.WriteLine("          ---------------------------------------------------\n");
                            Console.WriteLine("            0) Return to Main Menu    1) Make Borrowing Again");

                            string? Selected_MenuNumber = Console.ReadLine();
                            int ConvertedMenuNumber = Convert.ToInt32(Selected_MenuNumber);

                            if (ConvertedMenuNumber == 0)
                            {
                                ClearConsole();

                                UserInterface.MainMenu(this);
                            }
                            else if (ConvertedMenuNumber == 1)
                            {
                                BorrowBook();
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("          The book is borrowed!");
                            Console.ResetColor();
                            Console.WriteLine("          ---------------------------------------------------\n");
                            Console.WriteLine("            0) Return to Main Menu");

                            //Bir kitabın kopyalarını bitene kadar ödünç alabiliriz ama alım tarihi ilk kopyanın tarihi olur.
                            //Ve iade tarihi ilk alınan kopyadan hesaplanır.
                            book.Borrow_Number++;
                            book.Copy_Number--;
                            book.IsBorrowed = true;
                            book.Borrow_Date = DateTime.Today;
                            book.Due_Date = DateTime.Today.AddDays(5);

                            SaveBooks();

                            BackMainMenu();
                        }
                    }
                }
            }
        }
    }
    //Kitap iade etme fonksiyonu.
    public void BookReturn()
    {
        ClearConsole();
        LoadBooks();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("          List of Borrowed Books:\n");
        Console.ResetColor();
        foreach (var book in Books)
        {
            //Eğer bir kitap ödünç alınmışsa kaç kopya alındıysa her biri listelenir.
            if (book.IsBorrowed == true && DateTime.Today <= book.Due_Date)
            {
                for (int i = 1; i <= book.Borrow_Number; i++)
                {
                    Console.WriteLine("             Book Title: " + book.Name);
                    Console.WriteLine("             Book Author: " + book.Author);
                    Console.WriteLine("             Book ISBN Number: " + book.ISBN_Number);
                    Console.WriteLine("             Date Borrowed " + book.Borrow_Date.ToShortDateString());
                    Console.WriteLine("             Return Date " + book.Due_Date.ToShortDateString());
                    Console.WriteLine("          ---------------------------------------------------\n");
                }
            }
        }
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("          List of Books with Expired Return Deadline:\n");
        Console.ResetColor();
        foreach (var book in Books)
        {   //İade süresi dolmuş kitaplar ayrı listelenir.
            if (book.IsBorrowed == true && DateTime.Today > book.Due_Date)
            {
                for (int i = 1; i <= book.Borrow_Number; i++)
                {
                    Console.WriteLine("             Book Title: " + book.Name);
                    Console.WriteLine("             Book Author: " + book.Author);
                    Console.WriteLine("             Book ISBN Number: " + book.ISBN_Number);
                    Console.WriteLine("             Date Borrowed " + book.Borrow_Date.ToShortDateString());
                    Console.WriteLine("             Return Date " + book.Due_Date.ToShortDateString());
                    Console.WriteLine("          ---------------------------------------------------\n");
                }
            }
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("          You can return to the main menu without starting the process by typing 0.");
        Console.WriteLine("          The list of books you have borrowed that have expired/not expired is above.");
        Console.ResetColor();
        Console.WriteLine("          Enter the ISBN number of the book you are returning: ");
        string? Entered_ISBN_Number = Console.ReadLine();
        //ISBN hatırlanmadığı durumda ana menüye dönme fırsatı verir.
        if (Entered_ISBN_Number == "0")
        {
            UserInterface.MainMenu(this);
        }
        else
        {   //ISBN kontrol ifleri.
            if (long.TryParse(Entered_ISBN_Number, out long result) == false)
            {
                while (long.TryParse(Entered_ISBN_Number, out long result2) == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("          ISBN number must consist of numbers only");
                    Console.ResetColor();
                    Console.WriteLine("          Enter the ISBN number of the book you want to return again: ");
                    Entered_ISBN_Number = Console.ReadLine();
                }
            }
            else if (Entered_ISBN_Number.Length < 13 || Entered_ISBN_Number.Length > 13)
            {
                while (Entered_ISBN_Number.Length < 13 || Entered_ISBN_Number.Length > 13)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("          ISBN number must consist of 13 digits");
                    Console.ResetColor();
                    Console.WriteLine("          Enter the ISBN number of the book you want to return again: ");
                    Entered_ISBN_Number = Console.ReadLine();
                }
            }
            else
            {
                foreach (var book in Books)
                {
                    if (book.ISBN_Number == Entered_ISBN_Number && book.IsBorrowed)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("          Your book has been returned.");
                        Console.ResetColor();
                        Console.WriteLine("          ---------------------------------------------------\n");
                        Console.WriteLine("            0) Return to Main Menu    1) Make a Return Again");

                        book.Borrow_Number--;
                        book.Copy_Number++;

                        if (book.Borrow_Number == 0)
                        {
                            book.IsBorrowed = false;
                        }

                        SaveBooks();

                        string? Selected_MenuNumber = Console.ReadLine();
                        int ConvertedMenuNumber = Convert.ToInt32(Selected_MenuNumber);

                        if (ConvertedMenuNumber == 0)
                        {
                            ClearConsole();

                            UserInterface.MainMenu(this);
                        }
                        else if (ConvertedMenuNumber == 1)
                        {
                            BookReturn();
                        }
                    }
                }
            }
        }
    }
}
//Anamenü kontrol classı.
class UserInterface
{
    //Bir nevi void Start() fonksiyonu.
    static void Main()
    {
        Library library = new Library();
        MainMenu(library);
    }
    //Anamenü yazıları.
    public static void MainMenu(Library library)
    {
        library.ClearConsole();

        Console.WriteLine("          ---------------------------------------------------\n");
        Console.WriteLine("                     1) Book List\n");
        Console.WriteLine("                     2) Book Search\n");
        Console.WriteLine("                     3) Add Book\n");
        Console.WriteLine("                     4) Borrow Book\n");
        Console.WriteLine("                     5) Return Book\n");
        Console.WriteLine("                     0) Exit\n");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("                     9) Delete Data!!!\n");
        Console.ResetColor();
        Console.WriteLine("          ---------------------------------------------------\n");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("            You can use the menu with 0,1,2,3,4,5 buttons");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        //Verileri silmenin bir yoluda proje dosyaları içerisinde LibraryManagementSystem\bin\Books.txt dosyasını silmektir.
        Console.WriteLine("            Press button 9 to delete data");
        Console.ResetColor();

        string? Selected_MenuNumber = Console.ReadLine();
        int ConvertedMenuNumber = Convert.ToInt32(Selected_MenuNumber);
        //Anamenü üzerinden hangi menüye gidileceğini seçen kısım.
        switch (ConvertedMenuNumber)
        {
            case 0:
                Environment.Exit(0);
                break;
            case 1:
                library.ListBooks();
                break;
            case 2:
                library.SearchBook();
                break;
            case 3:
                library.AddBook();
                break;
            case 4:
                library.BorrowBook();
                break;
            case 5:
                library.BookReturn();
                break;
            case 9:
                library.DeleteBooks();
                MainMenu(library);
                break;
            default:
                Console.WriteLine("The menu you are trying to access was not found!");
                MainMenu(library);
                break;
        }
    }
}

