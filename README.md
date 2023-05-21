# BankingSystem.API 
Project was developed using ASP.NET Core Web API and includes a range of features such as online banking, ATM functionality, and financial reporting capabilities.

*აღწერა ქართულად იხილეთ ქვემოთ

# Banking System

### short description

The goal of the project is to create an API for the banking system, using which users will be able to transfer funds to their own or other accounts and use an ATM.

System managers will be able to view different types of reports.

The project includes several modules. Each module can be considered as an independent system, although some modules depend on the presence of other modules. Therefore, they should be implemented in sequence.

### modules

- Internet bank
     - operator
     - the user
- ATM client
     - Card authorization
     - ATM operations
- Reports
     - User statistics
     - Statistics of transactions

### Internet bank

The internet bank should be a web application, through which bank operators will be able to register users, create bank accounts for them, and add cards to bills.

**operator**

The operator must be able to register individuals, during which he will enter the following data:

- name *
- last name *
- personal number *
- date of birth *
- E-mail mail *
- password *

When creating a bank account for the user, the operator must be able to specify the following data:

- IBAN (the IBAN must be validated during registration. The operator cannot create an account with an invalid IBAN)
- Amount (amount on the account)
- Currency (Currency. Must be a selectable field. Possible values: GEL, USD, EUR)

During card registration, the operator must provide the following data:

- number of card
- name and surname
- card term (year, month)
- 3 digit CVV code (for online payments)
- 4-digit PIN code (to withdraw money from an ATM)

**User**

Registered users should be able to view the accounts and cards created for them by the operator.

The user should be able to perform two types of transactions from the internet bank:

- money transfer between own accounts, during which the transfer fee will be 0%
- transfer to another bank account, during which the transfer fee will be 1% + 0.5 (GEL/USD/EUR)

When transferring money, the currency exchange rates of the accounts must be taken into account. If the currency of one account is different from the currency of the other account, the amount must be converted according to the predetermined exchange rate.

### ATM client

An API needs to be created for the ATM client, where the user will be able to perform various operations after card authorization.

**Card Authorization**

Card authorization is also required to perform any ATM operation.

For this, the user must specify the card number and PIN code.

Authorization should not be successful if the card has expired.

**ATM Operations**

After authorization, it should be possible to perform the following operations:

- view the balance
- Withdraw money in GEL, USD or EUR currency
- Change pin code

ATM withdrawal fee should be 2% and within 24 hours it should be possible to withdraw a maximum of 10,000 GEL.

### reports

Bank managers should be able to view the following types of reports (the API should return results in JSON format):

- User statistics
     - Number of users registered in the current year
     - Number of registered users in the last year
     - Number of registered users in the last 30 days
- Statistics of transactions
     - Number of transactions made in the last 1 month/6 months/1 year
     - Amount of income received from transactions in the last 1 month/6 months/1 year (in GEL/USD/EUR)
     - average revenue from one transaction (in GEL/USD/EUR)
     - Number of transactions in the last month by days (chart)
     - Total amount of money withdrawn from the ATM

Since the project does not include UI, we decided to use SWAGGER do demonstrate abilities of the system, have a look: ![image](https://github.com/Lasha-Avalishvili/BankingSystem.API/assets/105679179/8e9a2ed3-07d3-479a-8f0f-dde021134e3a)


### მოკლე აღწერა

პროექტის მიზანია შეიქმნას საბანკო სისტემისთვის API, რომლის გამოყენებითაც მომხმარებლები შეძლებენ თანხების გადარიცხვას საკუთარ ან სხვა ანგარიშებზე და ATM-ით სარგებლობას.

სისტემის მენეჯერები კი შეძლებენ სხვადადასხვა ტიპის რეპორტების ნახვას.

პროექტი მოიცავს რამდენიმე მოდულს. თითოეული მოდული შეგვიძლია განვიხილოთ როგორც დამოუკიდებელი სისტემა, თუმცა ზოგიერთი მოდული დამოკიდებულია სხვა მოდულების არსებობაზე. ამიტომ მათი იმპლემენტაცია უნდა მოხდეს თანმიმდევრობით.

### მოდულები

- ინტერნეტბანკი
    - ოპერატორი
    - მომხმარებელი
- ATM კლიენტი
    - ბარათის ავტორიზაცია
    - ATM ოპერაციები
- რეპორტები
    - მომხმარებლების სტატისტიკა
    - ტრანზაქციების სტატისტიკა

### ინტერნეტბანკი

ინტერნეტბანკი უნდა იყოს ვებ აპლიკაცია, რომლის საშუალებითაც ბანკის ოპერატორებს შეეძლებათ მომხმარებლების რეგისტრაცია, მათთვის საბანკო ანგარიშების შექმნა და ანაგირშებზე ბარათების დამატება.

**ოპერატორი**

ოპერატორს უნდა შეეძლოს ფიზიკური პირების რეგისტრაცია, რომლის დროსაც შეიყვანს შემდეგ მონაცემებს:

- სახელი *
- გვარი *
- პირადი ნომერი *
- დაბადების თარიღი *
- ელ. ფოსტა *
- პაროლი *

მომხმარებლისთვის საბანკო ანგარიშის შექმნის დროს, ოპერატორს უნდა შეეძლოს მიუთითოს შემდეგი მონაცემები:

- IBAN (რეგისტრაციის დროს უნდა მოხდეს IBAN-ის ვალიდაცია. არავალიდური IBAN-ით ოპერატორმა ვერ უნდა შეძლოს ანგარიშის შექმნა)
- Amount (ანგარიშზე არსებული თანხა)
- Currency (ვალუტა. უნდა იყოს ასარჩევი ველი. შესაძლო მნიშვნელობები: GEL, USD, EUR)

ბარათის რეგისტრაციის დროს ოპერატორმა უნდა მიუთითოს შემდეგი მონაცემები:

- ბარათის ნომერი
- სახელი და გვარი
- ბარათის ვადა (წელი, თვე)
- 3 ნიშნა CVV კოდი (ონლაინ გადახდებისთვის)
- 4 ნიშნა PIN კოდი (ATM-დან თანხის გამოსატანად)

**მომხმარებელი**

დარეგისტრირებულ მომხმარებლებს უნდა შეეძლოთ ოპერატორის მიერ მათთვის შექმნილი ანგარიშების და ბარათების ნახვა. 

ინტერნეტბანკიდან მომხმარებელს უნდა შეეძლოს ორი ტიპის ტრანზაქციის შესრულება:

- საკუთარ ანგარიშებს შორის თანხის გადარიცხვა, რომლის დროსაც გადარიცხვის საკომისიო იქნება 0%
- ბანკის სხვა ანგარიშზე გადარიცხვა, რომლის დროსაც გადარიცხვის საკომისიო იქნება 1% + 0.5 (ლარი/დოლარი/ევრო)

თანხის გადარიცხვისას გათვალისწინებული უნდა იყოს ანგარიშების ვალუტების კურსები. თუ ერთი ანგარიშის ვალუტა განსხვავდება მეორე ანგარიშის ვალუტისგან, უნდა მოხდეს თანხის კონვერტაცია წინასწარ განსაზღვრული კურსის მიხედვით.

### ATM კლიენტი

ATM კლიენტისთვის საჭიროა შეიქმნას API, სადაც მომხმარებელს ბარათის ავტორიზაციის შემდეგ შეეძლება სხვადასხვა ოპერაციის ჩატარება.

**ბარათის ავტორიზაცია**

ATM-ის ნებისმიერი ოპერაციის ჩასატარებლად საჭიროა ასევე მოხდეს ბარათის ავტორიზაცია.

ამისათვის მომხმარებელმა უნდა მიუთითოს ბარათის ნომერი და PIN კოდი.

ავტორიზაცია არ უნდა იყოს წარმატებული, თუ ბარათი ვადაგასულია.

**ATM ოპერაციები**

ავტორიზაციის შემდეგ, შესაძლებელი უნდა იყოს შემდეგი ოპერაციების ჩატარება:

- ბალანსის ნახვა
- თანხის გამოტანა GEL, USD ან EUR ვალუტაში
- პინ კოდის შეცვლა

ATM-დან თანხის გამოტანის საკომისიო უნდა იყოს 2% და 24 საათის განმავლობაში შესაძლებელი უნდა იყოს მაქსიმუმ 10,000 ლარის გამოტანა.

### რეპორტები

ბანკის მენეჯერებს უნდა შეეძლოთ შემდეგი ტიპის რეპორტების ნახვა (API-მ უნდა დააბრუნოს შედეგები JSON ფორმატში):

- მომხმარებლების სტატისტიკა
    - მიმდინარე წელს დარეგისტრირებული მომხმარებლების რაოდენობა
    - ბოლო ერთი წლის განმავლობაში დარეგისტრირებული მომხმარებლების რაოდენობა
    - ბოლო 30 დღეში დარეგისტრირებული მომხმარებლების რაოდენობა
- ტრანზაქციების სტატისტიკა
    - ბოლო 1 თვეში/6 თვეში/1წელში განხორციელებული ტრანზაქციების რაოდენობა
    - ბოლო 1 თვეში/6 თვეში/1წელში ტრანზაქციებიდან მიღებული შემოსავალის მოცულობა (ლარში/დოლარში/ევროში)
    - საშუალოდ ერთი ტრანზაქციიდან მიღებული შემოსავალი (ლარში/დოლარში/ევროში)
    - ბოლო ერთ თვეში ტრანზაქციების რაოდენობა დღეების მიხედვით (ჩარტი)
    - ATM-დან გამოტანილი თანხის ჯამური რაოდენობა
