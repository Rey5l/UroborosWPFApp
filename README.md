# Отчет по практической работе №6 Часть 2  
**СОЗДАНИЕ АВТОМАТИЗИРОВАННОГО UNIT-ТЕСТА**

**Цель практической работы:** провести тестирование разработанных программных модулей авторизации и регистрации пользователей с использованием средств автоматизации Microsoft Visual Studio методом "белого ящика".

## 1. Репозиторий проекта
[Ссылка на GitHub репозиторий](https://github.com/Rey5l/UroborosWPFApp)

## 2. Структура базы данных
### Скриншот таблицы users:
![image](https://github.com/user-attachments/assets/59ec2174-42e6-4717-bdb6-aedf17ae2e29)



## 3. Результаты тестирования
### Скриншот обозревателя тестов:
![image](https://github.com/user-attachments/assets/f92305e9-9577-408e-88a6-95322f172cc8)



### Результаты тестирования

| Тестовый сценарий | Длительность | Статус |
|-------------------|--------------|--------|
| **UnitTestProject1 (всего)** | 21,3 с | - |
| **LoginTests** | 6,3 с | - |
| AuthTest | 6,3 с | Успех |
| **NegativeAuthTests** | 7,5 с | - |
| Auth_WithEmptyFields_ReturnsFalse | 2,5 с | Успех |
| Auth_WithInvalidLogin_ReturnsFalse | 3,1 с | Успех |
| Auth_WithInvalidPassword_ReturnsFalse | 511 мс | Успех |
| Auth_WithSpacesInCredentials_ReturnsFalse | 710 мс | Успех |
| Auth_WithToolLongCredentials_ReturnsFalse | 631 мс | Успех |
| **RegistrationTests** | 3,9 с | - |
| Register_WithEmptyFields_ReturnsError | 3,4 с | Успех |
| Register_WithExistingUsername_ReturnsError | 433 мс | Успех |
| Register_WithInvalidEmail_ReturnsError | 3 мс | Успех |
| Register_WithLongData_ReturnsError | 20 мс | Успех |
| Register_WithShortPassword_ReturnsError | 39 мс | Ошибка |
| Register_WithSpaces_ReturnsError | 5 мс | Ошибка |
| Register_WithSpecialCharacters_ReturnsSuccess | 11 мс | Успех |
| Register_WithValidData_ReturnsSuccess | 6 мс | Успех |
| **UnitTest1** | 1 мс | - |
| TestMethod1 | 1 мс | Успех |
| **UnitTest3** | 3,6 с | - |
| AuthTestSuccess | 3,6 с | Успех |




### **Вывод по результатам тестирования и рекомендации**

#### **1. Общая оценка результатов**  
Проведенное тестирование модулей авторизации и регистрации показало:  
✅ **Успешное выполнение 14 из 16 тестов (87,5%)** – система корректно обрабатывает:  
- Валидные и невалидные данные авторизации  
- Проверку уникальности логина и email  
- Формат email и длину полей  
- Специальные символы в данных  

❌ **Два неудачных теста (12,5%)**:  
- `Register_WithShortPassword_ReturnsError` (39 мс)  
- `Register_WithSpaces_ReturnsError` (5 мс)  

---

#### **2. Ключевые проблемы**  
1. **Несоответствие требованиям к паролю**:  
   - Тест ожидает проверку на **8 символов**, но код допускает **5 символов**.  
   - *Пример ошибки*:  
     ```plaintext
     Ожидалось: "Пароль должен содержать минимум 8 символов"  
     Фактически: "Длина пароля должна быть больше 5 символов"
     ```  

2. **Некорректная обработка пробелов**:  
   - Тест `Register_WithSpaces_ReturnsError` завершился ошибкой, хотя система должна отклонять логины/пароли с пробелами.  

---

#### **3. Рекомендации по исправлению**  

**Для разработчика:**  
1. **Обновить валидацию пароля** (в `RegisterPage.cs`):  
   ```csharp
   // Было: if (password.Length < 5)  
   if (password.Length < 8) // Исправлено на 8 символов
       return new RegistrationResult(false, "Пароль должен содержать минимум 8 символов");
   ```  

2. **Добавить проверку пробелов** (аналогично `LoginPage`):  
   ```csharp
   if (username.Contains(" ") || password.Contains(" "))
       return new RegistrationResult(false, "Логин и пароль не должны содержать пробелы");
   ```  

**Для тестировщика:**  
3. **Добавить тесты для граничных значений**:  
   - Пароль ровно 8 символов  
   - Максимальная длина логина/email (если ограничения есть в требованиях)  

---

#### **4. Заключение**  
Система **готова к релизу** после исправления указанных недочетов. 

**Риски без исправлений**:  
- Возможна регистрация с ненадежными паролями (5 символов вместо 8).  
- Пользователи могут создавать логины с пробелами, что усложнит авторизацию.   

**Итог**: 87,5% успешных тестов – система стабильна, но требует минимальных доработок


**После исправления всех недочётов:**

![Скриншот обозревателя тестов](https://github.com/user-attachments/assets/6eb728d9-5c9f-4471-bdd7-1de41aa3f612)

