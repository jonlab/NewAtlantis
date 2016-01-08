=======================================
==========   ZOMBIEGONS   =============
Modern : Concrete Building Exterior 01
Version : 2
=======================================

Description:
Generic modular concrete facade. Could be used for meshing large 
exterior buildings or walls. Could also be used for meshing
some basic industrial looking interiors.

Additional Notes:
    - Meshset uses a custom asset post processor (RWAssetPostProcessor)
    - Asset post processor applies properties from the meshset xml file 
    (Modern_ConcreteBuildingExt_01.xml) and calculates tangents from the
    second UV channel.
    - Tangents for meshset are generated from second UV channel because
    of the full object normalmap (Body Normalmap) baked using that channel.
    - Body normalmap is blended with a tiled material and it's what gives
    the meshset the appearance of smooth or beveled edges.
    
If you have any questions or need support feel free to post a comment on my blog or email me.
Blog : http://zombiegons.blogspot.com/
Email : zombiegons@gmail.com

========= Version Changes =============
Version 1:
	* Initial release.

Version 2:
	* 	Updated vertex colors on meshset because of some changes that were made
		Bumped Detail Spec 01 shader for Building Exterior 01 meshset.
		
	*	Updated the Shared assets and scripts that changed for Building Exterior 01 meshset.