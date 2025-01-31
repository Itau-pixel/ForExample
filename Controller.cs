using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Save;

public class Controller : MonoBehaviour
{
    [SerializeField] private Save Save;
    [SerializeField] private QuatesGenerate QuatesGenerate;
    [SerializeField] private GameObject Onboarding;

    [Header("display quote")]
    public ScrollRect _scroll;
    public Category categoryActive;

    [Header("Category")]
    public CategoryElement[] CategoryElements;
    [SerializeField] private Color activeColorButton, isactiveColorButton;
    [SerializeField] private Image[] ImageButtonsCategory;
    private int countQuoteCategory;

    [Header("Prefab quote")]
    public GameObject PrefabQuote, PrefabAbsenceQuote;

    [Header("Add quote")]
    private QuotesProperties ChangeProperties;
    private GameObject[] QuotesGM = new GameObject[0];

    [Header("Option quote")]
    public QuoteEdit QuoteEdit;
    public QuoteView QuoteView;
    public QuoteFavorite QuoteFavorite;
    public Color activeColorFavorite, isactiveColorFavorite;

    [Serializable]
    public class CategoryElement
    {
        public Sprite sprite;
        public string text;
    }
    public void Start()
    {
        Save.LoadGame();
        Onboarding.SetActive(Save.firstOpen);

        UpdateCategoryQuote(Category.All);
        QuoteFavorite.UpdateCategoryQuote();
    }
    public QuoteData FindIndexQuote(int instanceID)
    {
        return Save.Quotes.Find(x => x.ID == instanceID);
    }
    public void ButtonCategory(Image _image)
    {
        foreach (var elem in ImageButtonsCategory)
            elem.color = isactiveColorButton;

        if (categoryActive != (Category)Enum.Parse(typeof(Category), _image.gameObject.name))
        {
            _image.color = activeColorButton;
            categoryActive = (Category)Enum.Parse(typeof(Category), _image.gameObject.name);
        }
        else
            categoryActive = Category.All;

        UpdateCategoryQuote(categoryActive);
    }

    public int IndexCategory(Category _category)
    {
        int index = 0;
        for (int i = 0; i < CategoryElements.Length; i++)
        {
            if (CategoryElements[i].text == _category.ToString())
                index = i; //т.к. 1 категория - All
        }
        return index;
    }
    public void EndOnboarding()
    {
        Onboarding.SetActive(false);
        Save.firstOpen = false;
        Save.SaveGame();
    }

    public void DeleteQuote(int _id) {
        Save.Quotes.RemoveAt(_id);
        for (int i = 0; i < Save.Quotes.Count; i++)
            Save.Quotes[i].ID = i;
        Save.SaveGame();

        UpdateCategoryQuote(categoryActive);
        QuoteFavorite.UpdateCategoryQuote();
    }
    public void EditingQuote(int _id)
    {
        QuoteEdit.StartEditing(_id);
    }
    public void ViewingQuote(int _id)
    {
        QuoteView.StartViewing(_id);
    }
    public void EditFavorite(int _id, bool active)
    {
        Save.Quotes[_id].favorite = active;
        Save.SaveGame();
        UpdateCategoryQuote(categoryActive);
        QuoteFavorite.UpdateCategoryQuote();
    }
    public void UpdateCategoryQuote(Category category)
    {
        Save.LoadGame();
        foreach (var elem in QuotesGM)
            Destroy(elem);

        var content = _scroll.content;
        var targetTransform = content.transform;
        if (Save.Quotes.Count != 0)
            QuotesGM = new GameObject[Save.Quotes.Count];

        countQuoteCategory = 0;

        if (category == Category.All)
        {
            for (int i = 0; i < Save.Quotes.Count; i++)
            {
                ChangeProperties = Instantiate(PrefabQuote, targetTransform).GetComponent<QuotesProperties>();
                CreateQuoteElement(i);
            }
        }
        else
        {
            for (int i = 0; i < Save.Quotes.Count; i++)
            {
                if (Save.Quotes[i].category == category)
                {
                    ChangeProperties = Instantiate(PrefabQuote, targetTransform).GetComponent<QuotesProperties>();
                    CreateQuoteElement(i);
                }
            }
        }
        if (countQuoteCategory == 0)
        {
            QuotesGM = new GameObject[1];
            QuotesGM[0] = Instantiate(PrefabAbsenceQuote, targetTransform);
        }
    }

    public void CreateQuoteElement(int i)
    {
        ChangeProperties.textQuote.text = Save.Quotes[i].quoteText;
        ChangeProperties.authorQuote.text = Save.Quotes[i].author;
        ChangeProperties.date.text = Save.Quotes[i].date;
        ChangeProperties.categoryText.text = CategoryElements[IndexCategory(Save.Quotes[i].category)].text;
        ChangeProperties.categoryImage.sprite = CategoryElements[IndexCategory(Save.Quotes[i].category)].sprite;
        ChangeProperties.Controller = this;
        ChangeProperties.ID = Save.Quotes[i].ID;
        ChangeProperties.isFavorite = Save.Quotes[i].favorite;
        ChangeProperties.Favorite();

        QuotesGM[i] = ChangeProperties.gameObject;
        countQuoteCategory++;
    }
}
