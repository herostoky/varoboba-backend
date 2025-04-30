namespace VarobobaBackend.Core.VendorAggregate;

using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;

public class Vendor : EntityBase, IAggregateRoot
{
    private List<VendorPaymentMethod> _paymentMethods = new();
    private List<VendorDeliveryOption> _deliveryOptions = new();


    public Vendor(VendorId id,
                  UserId ownerId,
                  VendorName name,
                  Slug slug,
                  VendorDescription description)
    {
        Id = Guard.Against.Null(id, nameof(id));
        OwnerId = Guard.Against.Null(ownerId, nameof(ownerId));
        Name = Guard.Against.Null(name, nameof(name));
        Slug = Guard.Against.Null(slug, nameof(slug));
        Description = Guard.Against.Null(description, nameof(description));
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public new VendorId Id { get; private set; }
    public UserId OwnerId { get; private set; }
    public VendorName Name { get; private set; }
    public Slug Slug { get; private set; }
    public VendorDescription Description { get; private set; }
    public bool IsFeatured { get; private set; }
    public bool IsTemporarilyClosed { get; private set; }
    public CategoryId? CategoryId { get; private set; }
    public CategoryId? SubCategoryId { get; private set; }
    public VendorContactInfo? ContactInfo { get; private set; }
    public VendorLocation? Location { get; private set; }
    public VendorVisualIdentity? VisualIdentity { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public IReadOnlyCollection<VendorPaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();
    public IReadOnlyCollection<VendorDeliveryOption> DeliveryOptions => _deliveryOptions.AsReadOnly();

    public void Update(VendorName name, VendorDescription description, Slug slug)
    {
        Name = Guard.Against.Null(name, nameof(name));
        Description = Guard.Against.Null(description, nameof(description));
        Slug = Guard.Against.Null(slug, nameof(slug));
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCategory(CategoryId categoryId, CategoryId? subCategoryId = null)
    {
        CategoryId = Guard.Against.Null(categoryId, nameof(categoryId));
        SubCategoryId = subCategoryId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateContactInfo(VendorContactInfo contactInfo)
    {
        ContactInfo = Guard.Against.Null(contactInfo, nameof(contactInfo));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateLocation(VendorLocation location)
    {
        Location = Guard.Against.Null(location, nameof(location));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateVisualIdentity(VendorVisualIdentity visualIdentity)
    {
        VisualIdentity = Guard.Against.Null(visualIdentity, nameof(visualIdentity));
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetFeatured(bool isFeatured)
    {
        IsFeatured = isFeatured;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetTemporarilyClosed(bool isTemporarilyClosed)
    {
        IsTemporarilyClosed = isTemporarilyClosed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddPaymentMethod(PaymentProvider type, string identifier, string label)
    {
        _paymentMethods.Add(new VendorPaymentMethod(Guid.NewGuid(), Id, type, identifier, label));
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemovePaymentMethod(Guid paymentMethodId)
    {
        var method = _paymentMethods.Find(m => m.Id == paymentMethodId);
        if (method != null)
        {
            _paymentMethods.Remove(method);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void AddDeliveryOption(string deliveryHours, Money price, string estimatedTime, string coverageArea)
    {
        _deliveryOptions.Add(new VendorDeliveryOption(Guid.NewGuid(), Id, deliveryHours, price, estimatedTime, coverageArea));
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveDeliveryOption(Guid deliveryOptionId)
    {
        var option = _deliveryOptions.Find(o => o.Id == deliveryOptionId);
        if (option != null)
        {
            _deliveryOptions.Remove(option);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

public class VendorPaymentMethod
{
    public VendorPaymentMethod(Guid id, VendorId vendorId, PaymentProvider type, string identifier, string label)
    {
        Id = id;
        VendorId = Guard.Against.Null(vendorId, nameof(vendorId));
        Type = type;
        Identifier = Guard.Against.NullOrWhiteSpace(identifier, nameof(identifier));
        Label = label;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public VendorId VendorId { get; private set; }
    public PaymentProvider Type { get; private set; }
    public string Identifier { get; private set; }
    public string Label { get; private set; }
    public DateTime CreatedAt { get; private set; }
}

public class VendorDeliveryOption
{
    public VendorDeliveryOption(Guid id, VendorId vendorId, string deliveryHours, Money price, string estimatedTime, string coverageArea)
    {
        Id = id;
        VendorId = Guard.Against.Null(vendorId, nameof(vendorId));
        DeliveryHours = deliveryHours;
        Price = Guard.Against.Null(price, nameof(price));
        EstimatedTime = estimatedTime;
        CoverageArea = coverageArea;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public VendorId VendorId { get; private set; }
    public string DeliveryHours { get; private set; }
    public Money Price { get; private set; }
    public string EstimatedTime { get; private set; }
    public string CoverageArea { get; private set; }
    public DateTime CreatedAt { get; private set; }
}

// Value Objects
public record VendorId(Guid Value)
{
    public static implicit operator Guid(VendorId id) => id.Value;
    public static implicit operator VendorId(Guid id) => new(id);
}

public record UserId(Guid Value)
{
    public static implicit operator Guid(UserId id) => id.Value;
    public static implicit operator UserId(Guid id) => new(id);
}

public record CategoryId(Guid Value)
{
    public static implicit operator Guid(CategoryId id) => id.Value;
    public static implicit operator CategoryId(Guid id) => new(id);
}

public record VendorName
{
    public string Value { get; }

    public VendorName(string value)
    {
        Value = Guard.Against.NullOrWhiteSpace(value, nameof(value));
        if (value.Length < 3 || value.Length > 100)
            throw new ArgumentException("Vendor name must be between 3 and 100 characters", nameof(value));
    }

    public static implicit operator string(VendorName name) => name.Value;
}

public record Slug
{
    public string Value { get; }

    public Slug(string value)
    {
        Value = Guard.Against.NullOrWhiteSpace(value, nameof(value));
        if (!IsValidSlug(value))
            throw new ArgumentException("Invalid slug format. Use lowercase letters, numbers, and hyphens only.", nameof(value));
    }

    private bool IsValidSlug(string value)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(value, "^[a-z0-9]+(?:-[a-z0-9]+)*$");
    }

    public static implicit operator string(Slug slug) => slug.Value;
}

public record VendorDescription
{
    public string Value { get; }

    public VendorDescription(string value)
    {
        Value = value ?? string.Empty;
        if (value?.Length > 500)
            throw new ArgumentException("Description cannot exceed 500 characters", nameof(value));
    }

    public static implicit operator string(VendorDescription description) => description.Value;
}

public record Email
{
    public string Value { get; }

    public Email(string value)
    {
        Value = Guard.Against.NullOrWhiteSpace(value, nameof(value));
        if (!IsValidEmail(value))
            throw new ArgumentException("Invalid email format", nameof(value));
    }

    private bool IsValidEmail(string value)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(value);
            return addr.Address == value;
        }
        catch
        {
            return false;
        }
    }

    public static implicit operator string(Email email) => email.Value;
}

public record PhoneNumber
{
    public string Value { get; }

    public PhoneNumber(string value)
    {
        Value = Guard.Against.NullOrWhiteSpace(value, nameof(value));
        if (!IsValidPhoneNumber(value))
            throw new ArgumentException("Phone number should be in E.164 format", nameof(value));
    }

    private bool IsValidPhoneNumber(string value)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(value, @"^\+[1-9]\d{1,14}$");
    }

    public static implicit operator string(PhoneNumber phone) => phone.Value;
}

public record HexColor
{
    public string Value { get; }

    public HexColor(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            value = "#000000";

        if (!System.Text.RegularExpressions.Regex.IsMatch(value, "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$"))
            throw new ArgumentException("Invalid hex color format. Use #RGB or #RRGGBB format.", nameof(value));

        Value = value;
    }

    public static implicit operator string(HexColor color) => color.Value;
}

public record ImageUrl
{
    public string Value { get; }

    public ImageUrl(string value)
    {
        Value = value ?? string.Empty;
    }

    public static implicit operator string(ImageUrl url) => url.Value;
}

public record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "MGA")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        Amount = amount;
        Currency = currency;
    }
}

public record VendorContactInfo
{
    public Email Email { get; }
    public PhoneNumber PhoneNumber { get; }
    public string OpeningHours { get; }

    public VendorContactInfo(Email email, PhoneNumber phoneNumber, string openingHours)
    {
        Email = Guard.Against.Null(email, nameof(email));
        PhoneNumber = Guard.Against.Null(phoneNumber, nameof(phoneNumber));
        OpeningHours = openingHours ?? string.Empty;
    }
}

public record VendorLocation
{
    public string Address { get; }
    public string City { get; }
    public string Region { get; }

    public VendorLocation(string address, string city, string region)
    {
        Address = address ?? string.Empty;
        City = Guard.Against.NullOrWhiteSpace(city, nameof(city));
        Region = Guard.Against.NullOrWhiteSpace(region, nameof(region));
    }
}

public record VendorVisualIdentity
{
    public ImageUrl LogoUrl { get; }
    public ImageUrl BannerUrl { get; }
    public HexColor PrimaryColor { get; }
    public HexColor SecondaryColor { get; }
    public VendorTheme ThemeStyle { get; }

    public VendorVisualIdentity(ImageUrl logoUrl, ImageUrl bannerUrl, HexColor primaryColor, HexColor secondaryColor, VendorTheme themeStyle)
    {
        LogoUrl = logoUrl ?? new ImageUrl(string.Empty);
        BannerUrl = bannerUrl ?? new ImageUrl(string.Empty);
        PrimaryColor = primaryColor ?? new HexColor("#000000");
        SecondaryColor = secondaryColor ?? new HexColor("#FFFFFF");
        ThemeStyle = themeStyle;
    }
}

public enum PaymentProvider
{
    Telma,
    Airtel,
    Orange,
    Bank
}

public enum VendorTheme
{
    Modern,
    Classic,
    Minimalist,
    Vibrant,
    Elegant
}