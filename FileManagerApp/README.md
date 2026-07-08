# 📁 مدير الملفات Z-Pro

> مدير ملفات مجاني ومتكامل مثل ZArchiver - مبني بـ C# و .NET MAUI

![Platform](https://img.shields.io/badge/Platform-Android%20|%20iOS%20|%20Windows-blue)
![Language](https://img.shields.io/badge/Language-C%23-green)
![Framework](https://img.shields.io/badge/Framework-.NET%20MAUI-purple)
![License](https://img.shields.io/badge/License-Free-success)

## ✨ المميزات

### 📂 إدارة الملفات
- تصفح الملفات والمجلدات
- نسخ / قص / لصق / حذف / إعادة تسمية
- إنشاء ملفات ومجلدات جديدة
- البحث في الملفات
- المفضلة
- عرض شبكي أو قائمة

### ✏️ محرر النصوص
- دعم 20+ لغة برمجة (Python, JavaScript, C#, Java, SQL, HTML, CSS, ...)
- ترقيم الأسطر
- التفاف النص
- تغيير حجم الخط
- حفظ التغييرات

### 🖼️ عارض ومحرر الصور
- عرض جميع أنواع الصور
- تكبير / تصغير / تدوير
- تعديل السطوع والتباين والتشبع
- فلاتر جاهزة (حار، بارد، رمادي، عتيق، حيوي)

### 🎬 مشغل ومحرر الفيديو
- تشغيل الفيديو والصوت
- التحكم بالسرعة (0.25x - 2x)
- قص المقاطع
- تعديل السطوع والتباين
- التحكم بالصوت

### 🎨 الثيمات
8 ثيمات مختلفة:
- 🌙 داكن (Dark)
- ☀️ فاتح (Light)
- 🌌 منتصف الليل (Midnight)
- 🌲 غابة (Forest)
- 🌊 محيط (Ocean)
- 🌅 غروب (Sunset)
- 🌸 وردي (Rose)
- ✨ ذهبي (Golden)

### ⚙️ الإعدادات
- تغيير الثيم
- لون التمييز (8 ألوان)
- حجم الخط
- اللغة (العربية / English)
- إظهار الملفات المخفية
- وضع العرض

### 🌐 أيقونات الملفات
أيقونات مميزة لكل نوع:
- 🐍 Python | 📜 JavaScript | 💠 TypeScript
- 🟢 C# | ☕ Java | 🗄️ SQL
- 🌐 HTML | 🎨 CSS | 📋 JSON
- 🖼️ صور | 🎬 فيديو | 🎵 صوت
- 📦 أرشيف | 📄 مستندات

---

## 🚀 بناء التطبيق

### المتطلبات
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) مع:
  - .NET MAUI workload
  - Android SDK (لبناء APK)

### الخطوات

#### 1. استنساخ المشروع
```bash
git clone https://github.com/YOUR_USERNAME/FileManagerApp.git
cd FileManagerApp
```

#### 2. استعادة الحزم
```bash
dotnet restore
```

#### 3. بناء APK للأندرويد
```bash
dotnet publish -f net8.0-android -c Release
```

الـ APK سيكون في:
```
bin/Release/net8.0-android/publish/
```

#### 4. بناء للويندوز
```bash
dotnet publish -f net8.0-windows10.0.19041.0 -c Release
```

---

## 📱 التثبيت

### Android
1. انسخ ملف `.apk` إلى هاتفك
2. فعّل "التثبيت من مصادر غير معروفة"
3. افتح الملف وثبته

### Windows
1. شغل ملف `.exe` أو `.msix`

---

## 🏗️ هيكل المشروع

```
FileManagerApp/
├── Models/              # نماذج البيانات
│   ├── FileItem.cs
│   └── AppSettings.cs
├── ViewModels/          # MVVM ViewModels
│   ├── FileBrowserViewModel.cs
│   ├── TextEditorViewModel.cs
│   ├── ImageEditorViewModel.cs
│   ├── VideoEditorViewModel.cs
│   └── SettingsViewModel.cs
├── Views/               # صفحات XAML
│   ├── FileBrowserPage.xaml
│   ├── TextEditorPage.xaml
│   ├── ImageEditorPage.xaml
│   ├── VideoEditorPage.xaml
│   └── SettingsPage.xaml
├── Services/            # الخدمات
│   ├── FileService.cs
│   ├── SettingsService.cs
│   └── ThemeService.cs
├── Converters/          # محولات XAML
├── Resources/           # الموارد
│   ├── Styles/
│   ├── Fonts/
│   └── Images/
└── Platforms/           # كود المنصات
    ├── Android/
    ├── iOS/
    └── Windows/
```

---

## 🤝 المساهمة

نرحب بمساهماتكم! يمكنكم:
1. Fork المشروع
2. إنشاء فرع جديد (`git checkout -b feature/AmazingFeature`)
3. Commit التغييرات (`git commit -m 'Add some AmazingFeature'`)
4. Push للفرع (`git push origin feature/AmazingFeature`)
5. فتح Pull Request

---

## 📄 الترخيص

مجاني ومفتوح المصدر 💚

---

## 📞 الدعم

إذا واجهت أي مشكلة، افتح Issue على GitHub.

---

<div align="center">
  <h3>صُنع بـ ❤️ للمستخدمين</h3>
  <p>مجاني للجميع • بدون إعلانات • بدون رسوم</p>
</div>
