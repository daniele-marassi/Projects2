package crc6485961bf0292dc4a3;


public class WebChromeClientOverride
	extends android.webkit.WebChromeClient
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Supp.App.WebChromeClientOverride, Supp.App", WebChromeClientOverride.class, __md_methods);
	}


	public WebChromeClientOverride ()
	{
		super ();
		if (getClass () == WebChromeClientOverride.class) {
			mono.android.TypeManager.Activate ("Supp.App.WebChromeClientOverride, Supp.App", "", this, new java.lang.Object[] {  });
		}
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
