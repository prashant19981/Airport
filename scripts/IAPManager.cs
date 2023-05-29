using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour , IStoreListener {

	public static IAPManager instance;
	private static IStoreController m_StoreController;          // The Unity Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

	public static string product_coins_5000 = "coins_5000";
	public static string product_coins_10000 = "coins_10000";
	public static string product_no_Ads = "remove_ads";
	public static string product_4_crash = "crash_4";
	public static string product_unlimited_ILS = "unlimited_ils";
	public static string product_unlock_all = "unlock_all_levels";
	public Text price5000;
	public Text price10000;
	public Text priceILS;
	public Text price4Crash;
	public Text priceAllLevels;
	void Awake(){
		if (instance == null) {
			instance = this;
		}
	
	}



	void Start()
	{
		// If we haven't set up the Unity Purchasing reference
		if (m_StoreController == null)
		{
			// Begin to configure our connection to Purchasing
			InitializePurchasing();
		}

	}
	void Update(){
		price5000.text = m_StoreController.products.WithID (product_coins_5000).metadata.localizedPriceString;
		price10000.text = m_StoreController.products.WithID (product_coins_10000).metadata.localizedPriceString;
		price4Crash.text = m_StoreController.products.WithID (product_4_crash).metadata.localizedPriceString;
		priceAllLevels.text = m_StoreController.products.WithID (product_unlock_all).metadata.localizedPriceString;
		priceILS.text = m_StoreController.products.WithID (product_unlimited_ILS).metadata.localizedPriceString;
	}
	public void Buy5000()
	{

		BuyProductID(product_coins_5000);
	}
	public void Buy10000()
	{

		BuyProductID(product_coins_10000);
	}
	public void Buynoads()
	{

		BuyProductID(product_no_Ads);
	}
	public void Buy4crash()
	{

		BuyProductID(product_4_crash);
	}
	public void BuyUnlimitedILS()
	{

		BuyProductID(product_unlimited_ILS);
	}
	public void BuyAllLevels()
	{

		BuyProductID(product_unlock_all);
	}

	public void InitializePurchasing() 
	{
		// If we have already connected to Purchasing ...
		if (IsInitialized())
		{
			// ... we are done here.
			return;
		}


		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());


		builder.AddProduct(product_coins_5000, ProductType.Consumable);
		builder.AddProduct(product_coins_10000, ProductType.Consumable);
		// Continue adding the non-consumable product.
		builder.AddProduct(product_no_Ads, ProductType.NonConsumable);
		builder.AddProduct (product_4_crash, ProductType.NonConsumable);
		builder.AddProduct (product_unlimited_ILS, ProductType.NonConsumable);
		builder.AddProduct (product_unlock_all, ProductType.NonConsumable);
		// Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
		// and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
		UnityPurchasing.Initialize(this, builder);
	}


	private bool IsInitialized()
	{
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}






	void BuyProductID(string productId)
	{
		// If Purchasing has been initialized ...
		if (IsInitialized())
		{
			// ... look up the Product reference with the general product identifier and the Purchasing 
			// system's products collection.
			Product product = m_StoreController.products.WithID(productId);

			// If the look up found a product for this device's store and that product is ready to be sold ... 
			if (product != null && product.availableToPurchase)
			{
				Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
				// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
				// asynchronously.
				m_StoreController.InitiatePurchase(product);
			}
			// Otherwise ...
			else
			{
				// ... report the product look-up failure situation  
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}
		// Otherwise ...
		else
		{
			// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
			// retrying initiailization.
			Debug.Log("BuyProductID FAIL. Not initialized.");
		}
	}




	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;
	}


	public void OnInitializeFailed(InitializationFailureReason error)
	{
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}


	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
	{
		// A consumable product has been purchased by this user.
		if (String.Equals (args.purchasedProduct.definition.id, product_coins_5000, StringComparison.Ordinal)) {
			saveData.loadCoins ();
			saveData.c.setCoins (saveData.c.getCoins () + 5000);
			saveData.saveCoins ();
		} else if (String.Equals (args.purchasedProduct.definition.id, product_coins_10000, StringComparison.Ordinal)) {
			saveData.loadCoins ();
			saveData.c.setCoins (saveData.c.getCoins () + 10000);
			saveData.saveCoins ();
			
		}
		// Or ... a non-consumable product has been purchased by this user.
		else if (String.Equals (args.purchasedProduct.definition.id, product_no_Ads, StringComparison.Ordinal)) {
			saveData.loadCoins ();
			saveData.c.setAdsPurchase (true);
			saveData.saveCoins ();
		} else if (String.Equals (args.purchasedProduct.definition.id, product_4_crash, StringComparison.Ordinal)) {
			saveData.loadCoins ();
			saveData.c.setIs4crashPurchased (true);
			saveData.saveCoins ();
		} else if (String.Equals (args.purchasedProduct.definition.id, product_unlimited_ILS, StringComparison.Ordinal)) {
			saveData.loadCoins ();
			saveData.c.setIsILSPurchased (true);
			saveData.saveCoins ();
		} else if (String.Equals (args.purchasedProduct.definition.id, product_unlock_all, StringComparison.Ordinal)) {
			saveData.loadlevelInfo ();
			for (int i = 0; i < 4; i++) {
				saveData.l [i].setStatus (true);
			}
			saveData.saveLevelInfo ();
		}
		else 
		{
			Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
		}


		return PurchaseProcessingResult.Complete;
	}


	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
		// this reason with the user to guide their troubleshooting actions.
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
	}
}
