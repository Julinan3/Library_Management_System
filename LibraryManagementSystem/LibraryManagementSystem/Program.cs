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
            Console.WriteLine("          Veri tabanına ekli kitap bulunmamaktadır !\n          Ana menü/Veri Tabanına Kitap Ekleme kısmından ekleyiniz. ");
            Console.ResetColor();
            Console.WriteLine("            0) Ana Menüye Dön");

            BackMainMenu();
        }
        else//Eğer varsa kitapları listeler.
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("          Kitapların Listesi:\n");
            Console.ResetColor();
            foreach (var book in Books)
            {
                Console.WriteLine("             Kitap Adı: " + book.Name);
                Console.WriteLine("             Kitap Yazarı: " + book.Author);
                Console.WriteLine("             Kitap ISBN Numarası: " + book.ISBN_Number);
                Console.WriteLine("             Kitap Kopya Sayısı: " + book.Copy_Number);
                Console.WriteLine("             Kitap Ödünç Alınma Sayısı: " + book.Borrow_Number);
                Console.WriteLine("          ---------------------------------------------------\n");
            }
            Console.WriteLine("            0) Ana Menüye Dön");

            BackMainMenu();
        }

    }
    //Veri tabanına eklenmiş kitapları arasında arama yapan fonksiyon.
    public void SearchBook()
    {
        LoadBooks();
        ClearConsole();

        Console.WriteLine("          ---------------------------------------------------\n");
        Console.WriteLine("          Aramak istediğiniz kitabın adını veya yazarını giriniz: ");

        string? BookNameOrAuthor = Console.ReadLine();

        foreach (var book in Books)
        {
            if(book.Name == BookNameOrAuthor || book.Author == BookNameOrAuthor)
            {
                Console.WriteLine("             Kitap Adı: " + book.Name);
                Console.WriteLine("             Kitap Yazarı: " + book.Author);
                Console.WriteLine("             Kitap ISBN Numarası: " + book.ISBN_Number);
                Console.WriteLine("             Kitap Kopya Sayısı: " + book.Copy_Number);
                Console.WriteLine("             Kitap Ödünç Alınma Sayısı: " + book.Borrow_Number);
                Console.WriteLine("          ---------------------------------------------------\n");
            }
        }
        Console.WriteLine("            0) Ana Menüye Dön    1) Tekrar Ara");

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
        Console.WriteLine("          Eklemek istediğiniz kitabın adı: ");
        string? Entered_Name = Console.ReadLine();
        Console.WriteLine("          Eklemek istediğiniz kitabın yazarı: ");
        string? Entered_Author = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("          ISBN numarası sayılardan oluşmalıdır.\n          13 basamaklı olmalıdır.\n          Veri sisteminde zaten bulunan bir ISBN numarası girilmemelidir.\n");
        Console.ResetColor();
        Console.WriteLine("          Eklemek istediğiniz kitabın ISBN numarası: ");
        string? Entered_ISBN_Number = Console.ReadLine();
        //Girilen ISBN numarasının uygunluğu için uzun kontrol ifleri.
        if (long.TryParse(Entered_ISBN_Number,out long result) == false)
        {
            while(long.TryParse(Entered_ISBN_Number, out long result2) == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("          ISBN numarası sadece sayılardan oluşmalıdır");
                Console.ResetColor();
                Console.WriteLine("          Eklemek istediğiniz kitabın ISBN numarasını tekrar giriniz: ");
                Entered_ISBN_Number = Console.ReadLine();
            }   
        }
        else if(Entered_ISBN_Number.Length < 13 || Entered_ISBN_Number.Length > 13)
        {
            while(Entered_ISBN_Number.Length < 13 || Entered_ISBN_Number.Length > 13)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("          ISBN numarası 13 basamaktan oluşmalıdır");
                Console.ResetColor();
                Console.WriteLine("          Eklemek istediğiniz kitabın ISBN numarasını tekrar giriniz: ");
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
                    Console.WriteLine("          Eklemek istediğiniz kitabın ISBN numarası daha önce kullanılmış");
                    Console.ResetColor();
                    Console.WriteLine("          Eklemek istediğiniz kitabın ISBN numarasını tekrar giriniz: ");
                    Entered_ISBN_Number = Console.ReadLine();
                    if(book.ISBN_Number == Entered_ISBN_Number) 
                    {
                        AddBook();
                    }
                }
            }
        }

        Console.WriteLine("          Eklemek istediğiniz kitabın bulunucak kopya sayısı: ");
        string? Entered_Copy_Number = Console.ReadLine();
        Console.WriteLine("          Kitap veritabanına eklendi!");
        Console.WriteLine("          ---------------------------------------------------\n");
        Books.Add(new Book(Entered_Name, Entered_Author, Entered_ISBN_Number, Convert.ToInt32(Entered_Copy_Number)));
        Console.WriteLine("            0) Ana Menüye Dön");

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
        Console.WriteLine("          Şuanda 0 yazarak işleme başlamadan ana menüye dönebilirsiniz.");
        Console.ResetColor();
        Console.WriteLine("          Ödünç aldığınız bir kitabı 5 gün içinde iade etmelisiniz.");
        Console.WriteLine("          Ödünç almak istediğiniz kitabın ISBN numarasını giriniz: ");
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
                    Console.WriteLine("          ISBN numarası sadece sayılardan oluşmalıdır");
                    Console.ResetColor();
                    Console.WriteLine("          Ödünç almak istediğiniz kitabın ISBN numarasını tekrar giriniz: ");
                    Entered_ISBN_Number = Console.ReadLine();
                }
            }
            else if (Entered_ISBN_Number.Length < 13 || Entered_ISBN_Number.Length > 13)
            {
                while (Entered_ISBN_Number.Length < 13 || Entered_ISBN_Number.Length > 13)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("          ISBN numarası 13 basamaktan oluşmalıdır");
                    Console.ResetColor();
                    Console.WriteLine("          Ödünç almak istediğiniz kitabın ISBN numarasını tekrar giriniz: ");
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
                            Console.WriteLine("          Ödünç alınabilicek kitap kopyası yok.");
                            Console.ResetColor();
                            Console.WriteLine("          ---------------------------------------------------\n");
                            Console.WriteLine("            0) Ana Menüye Dön    1) Tekrar Ödünç Alma Yap");

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
                            Console.WriteLine("          Kitap ödünç alındı!");
                            Console.ResetColor();
                            Console.WriteLine("          ---------------------------------------------------\n");
                            Console.WriteLine("            0) Ana Menüye Dön");

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
        Console.WriteLine("          Ödünç Aldığın Kitapların Listesi:\n");
        Console.ResetColor();
        foreach (var book in Books)
        {
            //Eğer bir kitap ödünç alınmışsa kaç kopya alındıysa her biri listelenir.
            if (book.IsBorrowed == true && DateTime.Today <= book.Due_Date)
            {
                for (int i = 1; i <= book.Borrow_Number; i++)
                {
                    Console.WriteLine("             Kitap Adı: " + book.Name);
                    Console.WriteLine("             Kitap Yazarı: " + book.Author);
                    Console.WriteLine("             Kitap ISBN Numarası: " + book.ISBN_Number);
                    Console.WriteLine("             Ödünç Alınma Tarihi " + book.Borrow_Date.ToShortDateString());
                    Console.WriteLine("             İade Edilme Tarihi " + book.Due_Date.ToShortDateString());
                    Console.WriteLine("          ---------------------------------------------------\n");
                }
            }
        }
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("          İade Süresi Dolmuş Kitapların Listesi:\n");
        Console.ResetColor();
        foreach (var book in Books)
        {   //İade süresi dolmuş kitaplar ayrı listelenir.
            if (book.IsBorrowed == true && DateTime.Today > book.Due_Date)
            {
                for (int i = 1; i <= book.Borrow_Number; i++)
                {
                    Console.WriteLine("             Kitap Adı: " + book.Name);
                    Console.WriteLine("             Kitap Yazarı: " + book.Author);
                    Console.WriteLine("             Kitap ISBN Numarası: " + book.ISBN_Number);
                    Console.WriteLine("             Ödünç Alınma Tarihi " + book.Borrow_Date.ToShortDateString());
                    Console.WriteLine("             İade Edilme Tarihi " + book.Due_Date.ToShortDateString());
                    Console.WriteLine("          ---------------------------------------------------\n");
                }
            }
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("          Şuanda 0 yazarak işleme başlamadan ana menüye dönebilirsiniz.");
        Console.WriteLine("          Ödünç aldığınız iade süresi dolmuş/dolmamış kitapların listesi yukardadır.");
        Console.ResetColor();
        Console.WriteLine("          İade ediceğiniz kitabın ISBN numarasını giriniz: ");
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
                    Console.WriteLine("          ISBN numarası sadece sayılardan oluşmalıdır");
                    Console.ResetColor();
                    Console.WriteLine("          İade ediceğiniz kitabın ISBN numarasını tekrar giriniz: ");
                    Entered_ISBN_Number = Console.ReadLine();
                }
            }
            else if (Entered_ISBN_Number.Length < 13 || Entered_ISBN_Number.Length > 13)
            {
                while (Entered_ISBN_Number.Length < 13 || Entered_ISBN_Number.Length > 13)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("          ISBN numarası 13 basamaktan oluşmalıdır");
                    Console.ResetColor();
                    Console.WriteLine("          İade ediceğiniz kitabın ISBN numarasını tekrar giriniz: ");
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
                        Console.WriteLine("          Kitabınız İade Edildi.");
                        Console.ResetColor();
                        Console.WriteLine("          ---------------------------------------------------\n");
                        Console.WriteLine("            0) Ana Menüye Dön    1) Tekrar İade Etme Yap");

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
        Console.WriteLine("                     1) Kitap Listesi Görüntüleme\n");
        Console.WriteLine("                     2) Kitap Listesinde Arama Yapma\n");
        Console.WriteLine("                     3) Veri Tabanına Kitap Ekleme\n");
        Console.WriteLine("                     4) Kitap Ödünç Alma\n");
        Console.WriteLine("                     5) Kitap İade Etme\n");
        Console.WriteLine("                     0) Çıkış Yapma\n");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("                     9) Verileri Sil!!!\n");
        Console.ResetColor();
        Console.WriteLine("          ---------------------------------------------------\n");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("            Menüyü 0,1,2,3,4,5 tuşları ile kullanabilirsiniz");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        //Verileri silmenin bir yoluda proje dosyaları içerisinde LibraryManagementSystem\bin\Books.txt dosyasını silmektir.
        Console.WriteLine("            9 numaralı tuş ile verileri silebilirsiniz");
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
                Console.WriteLine("Hatalı Giriş Yaptınız");
                MainMenu(library);
                break;
        }
    }
}

