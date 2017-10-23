package Dal;

import Interface.ICloudRepository;
import Model.CloudObject;

public class ReferentielManager {
	
	private static ReferentielManager instance = null;
	
	private ICloudRepository _cloudRepository;
	//Declarer les interfaces
	
	private ReferentielManager(){
		
		_cloudRepository = new ICloudRepository();
		//ajouter les interfaces ici
	}
	
	//Singleton
	public synchronized static ReferentielManager Instance()
	{
		//Synchronisation Globale
		if(instance == null)
		{
			instance = new ReferentielManager();
		}
		return instance;
	}


	public void GetInfosCloud(CloudObject c)
	{
		_cloudRepository.getCloud(c);
	}
	
}