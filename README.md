# Postup ke spuštění:
1. Stáhněte si zip soubor
2. Soubor Extrahujte
3. Spusťte aplikaci: "Rozvrh.exe"


# Postup ke zhodnocení vlastního rozvrhu:
1. Otevřete Rozvrh\bin\Debug\net6.0\JsonFiles\oldschedule.json
2. Soubor musí mít celkově 50 hodin, včetně hodnot null(volné hodiny)
možné vložit: null nebo hodina 
příklad

null - Volná hodina

{ "SubjectName": "PV ", "Teacher": "Mgr. Alena Reichlova & Ing. Ondrej Mandik", "Hours": 2, "Class": ["18b"], "TypeOfLecture": "Excercise", "Floor": ["3"]}

SubjectName - Název předmětu
Teacher - Vyučující daného předmětu, pokud učí předmět dva učitelé, tak se píší oba. Učitelé se píší i s tituly dle stránek školy.
Hours - Počet hodin daného předmětu v celým týdnu
Class - List učeben, zde se může vložit více učeben například ["18b","19b"], generátor poté vybírá různé třídy 
TypeOfLecture - Theory (M,A,C,...), Physical Excercise (TV), Excercise (PV,CIT,...)
Floor - List podlaží, na kterém se učebna nachází například pokud máme učebny ["18b", "19b"] tak Floor bude ["3","3"], protože oboje učebny jsou na patře tři


3. Spusťte aplikaci: "Rozvrh.exe"
4. První vypsaný rozvrh v programu je Vámi daný rozvrh


# Konfigurace
-  konfigurace probíhá po spuštění programu (nastavení času)


DOKUMENTACE PROJEKTU

Třída: Program
Hlavní třída programu je zodpovědná za generování a hodnocení školních rozvrhů.

Proměnné
random: Instance třídy Random pro generování náhodných hodnot.
reviewer: Instance třídy Reviewer pro hodnocení rozvrhů.
json: Řetězec obsahující data načtená ze souboru "JsonFiles/data.json".
schedules: Seznam obsahující listy dnů, představující vygenerované rozvrhy.
bestSchedules: Seznam obsahující trojice, kde první prvek je nejlepší rozvrh a druhý prvek je jeho hodnocení.
betterSchedules: Počet lepších rozvrhů než ten původní.
oldSchedulePoints: Hodnocení původního rozvrhu.
endallthreads: Logická hodnota určující ukončení všech vláken.
time: Časový limit pro běh programu.
schedulesCount: Celkový počet vygenerovaných rozvrhů.
reviewedCount: Celkový počet vyhodnocených rozvrhů.
lockObject: Objekt pro zajištění vzájemné vyloučenosti při přístupu ke sdíleným prostředkům.

Metody
Metoda: Main
Hlavní metoda programu, která řídí tok programu. Zahrnuje inicializaci, generování a hodnocení rozvrhů.

Metoda: CreateDay
Vytváří instanci třídy Day s vygenerovaným rozvrhem na základě poskytnutých předmětů, celkového počtu hodin a prázdných hodin.

Metoda: CreateWeek
Generuje týdenní rozvrh s náhodným rozložením hodin na základě dostupných předmětů.

Metoda: CalculateDayMaxHours
Slouží k rozdělení hodin ve školním rozvrhu na všechny dny tak, aby pátek nebyl bez hodin.

Metoda: Generator
Nekonečně generuje nové rozvrhy a přidává je do sdíleného seznamu.

Metoda: Review
Provádí hodnocení vygenerovaných rozvrhů a ukládá nejlepší do seznamu.

Metoda: PrintSchedule
Vizualizuje rozvrh v konzoli pro lepší čitelnost.

Metoda: Watchdog
Zajišťuje ukončení všech vláken po uplynutí časového limitu.


Třída: Day
Tato třída představuje jeden den ve školním rozvrhu a obsahuje denní harmonogram lekcí.

Proměnné
subjects: Seznam předmětů pro daný den.
totalHours: Celkový počet dostupných hodin pro denní rozvrh.
emptyhours: Počet prázdných hodin ve rozvrhu.
previousSubject: Název předchozího předmětu (interní účel).
previousType: Typ předchozí lekce (interní účel).
dailySchedule: Seznam lekcí reprezentujících denní rozvrh.
Day(List<Subject> subjects, int totalHours, int emptyhours): Inicializuje novou instanci třídy s určenými předměty, celkovým počtem hodin a počtem prázdných hodin.
Day(List<Lesson> dailySchedule): Inicializuje novou instanci třídy s předdefinovaným denním rozvrhem.

Metoda: GenerateTimetable
GenerateTimetable(): Celý generátor, který slouží k vytváření všech rozvrhů. Generuje náhodně rozložený rozvrh na základě dostupných předmětů.

Metoda: GetDailySchedule
GetDailySchedule(): Vrací denní rozvrh lekcí.


Třída: Reviewer
Tato interní třída představuje hodnotitele školních rozvrhů.

Metoda: ReviewWeek
ReviewWeek(List<Day> week): Tato metoda zajišťuje hodnocení všech dnů v týdnu.
Metoda: ReviewDay
ReviewDay(Day d): Tato metoda hodnotí jeden den podle určených kritérií.
Metody hodnocení (Rating1 - Rating10)
Rating1: Hodnotí, zda začínáte školu později. Získává body za začátek po 8. hodině a penalizuje za délku školy.

Rating2: Hodnotí, zda se stejný předmět neopakuje vícekrát v den (vyjma cvičení).

Rating3: Hodnotí, zda při přechodech mezi hodinami dochází k změně patra nebo třídy.

Rating4: Hodnotí, zda je mezi 5. a 8. hodinou volná hodina na oběd.

Rating5: Hodnotí, zda je denní rozvrh v rozumném rozsahu (5-8 hodin).

Rating6: Hodnotí, zda jsou cvičení v jednom dni postupně.

Rating7: Hodnotí, zda matematika a profilové předměty nejsou na začátku nebo po obědě.

Rating8: Hodnotí, zda není v jednom dni více než tři cvičení.

Rating9: Hodnotí, zda není několik hodin za sebou ve stylu "hodina - volná hodina - hodina - volná hodina - hodina".

Rating10: Hodnotí, zda den obsahuje učitele s negativním nebo pozitivním vlivem na pohodu.



Třída: OurSchoolSchedule
Tato třída slouží k vytváření školních rozvrhů na základě dat uložených v JSON souboru.

Metoda: CreateOurSchedule
CreateOurSchedule(): List<Day>
Tato metoda vytváří školní rozvrh na základě dat uložených v JSON souboru.

List<Day> - Seznam dnů, přičemž každý den obsahuje rozvrh lekcí.

Postup:
Získání cesty k JSON souboru obsahujícímu data o lekcích.
Deserializace obsahu JSON souboru na seznam objektů typu Lesson.
Rozdělení seznamu lekcí do denních rozvrhů.
Vytvoření seznamu objektů typu Day na základě denních rozvrhů.
Přiřazení denního rozvrhu k jednotlivým objektům typu Day.
Návrat seznamu dnů s odpovídajícími rozvrhy.


Třídy: Subject a Lesson
Tyto třídy jsou využívané k deserializaci json souborů nebo také pro generátor
