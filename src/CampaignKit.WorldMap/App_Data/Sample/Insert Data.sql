-- Script Date: 2019-01-28 7:18 PM  - ErikEJ.SqlCeScripting version 3.5.2.80
INSERT INTO [Maps] 
   ([MapId], 
   [AdjustedSize], 
   [ContentType], 
   [Copyright],  
   [FileExtension], 
   [MaxZoomLevel], 
   [Name], 
   [RepeatMapInX], 
   [UserId], 
   [WorldFolderPath], 
   [ThumbnailPath], 
   [MarkerData], 
   [Secret], 
   [IsPublic]) 
VALUES
   (
      1, 4000, 'image/png', '', '.png', 
      4, 'Sample', 0, '', 
      'C:\Users\mf1939\source\repos\open-campaign-logger\world-map\src\CampaignKit.WorldMap\wwwroot\world\1', '~/world/1/0/zoom-level.png', '[
 { "options": { "stroke": true, "color": " # f06eaa", "weight": 4, "opacity": 0.5, "fill": true, "fillColor": null, "fillOpacity": 0.2, "clickable": true }, "properties": { "title": "Rectangle Title", "layerType": "rectangle", "latlng": "(29.97, 31.12)", "id": "2997_3112", "content": { "ops": [ { "insert": "Rectangle Description" }, { "attributes": { "header": 1 }, "insert": "\n" }, { "insert": "\n" }, { "attributes": { "underline": true, "italic": true, "bold": true }, "insert": "This is the description." }, { "insert": "\n" } ] } }, "latlngs": [ { "lat": 29.974044591534422, "lng": 31.122508049011234 }, { "lat": 29.97731598122696, "lng": 31.122508049011234 }, { "lat": 29.97731598122696, "lng": 31.127400398254398 }, { "lat": 29.974044591534422, "lng": 31.127400398254398 } ] }, { "options": { "stroke": true, "color": " # f06eaa", "weight": 4, "opacity": 0.5, "fill": true, "fillColor": null, "fillOpacity": 0.2, "clickable": true }, "properties": { "title": "Polygon Title", "layerType": "polygon", "latlng": "(29.98, 31.13)", "id": "2998_3113", "content": { "ops": [ { "insert": "Polygon Description" }, { "attributes": { "header": 1 }, "insert": "\n" }, { "insert": "\nThis is the description.\n" } ] } }, "latlngs": [ { "lat": 29.98497358578796, "lng": 31.129674911499027 }, { "lat": 29.98497358578796, "lng": 31.129674911499027 }, { "lat": 29.981033338736204, "lng": 31.13113403320313 }, { "lat": 29.981033338736204, "lng": 31.13113403320313 }, { "lat": 29.98244589810904, "lng": 31.141390800476078 }, { "lat": 29.98244589810904, "lng": 31.141390800476078 }, { "lat": 29.989619902644307, "lng": 31.14036083221436 }, { "lat": 29.989619902644307, "lng": 31.14036083221436 } ] }, { "options": { "stroke": true, "color": " # f06eaa", "weight": 4, "opacity": 0.5, "fill": true, "fillColor": null, "fillOpacity": 0.2, "clickable": true, "radius": 373.866398252301 }, "properties": { "title": "Circle Title", "layerType": "circle", "latlng": "(29.98, 31.14)", "id": "2998_3114", "content": { "ops": [ { "attributes": { "link": "https: // www.cbc.ca" }, "insert": "Circle Description" }, { "attributes": { "header": 1 }, "insert": "\n" }, { "insert": "\nThis is the description.\n" } ] } }, "latlngs": { "lat": 29.97545725029985, "lng": 31.13555431365967 } }, { "options": { "icon": { "options": {}, "_initHooksCalled": true } }, "properties": { "title": "Marker Title", "layerType": "marker", "latlng": "(29.98, 31.14)", "id": "2998_3114", "content": { "ops": [ { "insert": "Marker Description" }, { "attributes": { "header": 1 }, "insert": "\n" }, { "insert": "\nThis is the description." }, { "attributes": { "list": "bullet" }, "insert": "\n" }, { "insert": "Another line" }, { "attributes": { "list": "bullet" }, "insert": "\n" } ] } }, "latlngs": { "lat": 29.97631227084605, "lng": 31.143836975097656 } } ] ',
 	'lNtqjEVQ', 1
   )
;

-- Script Date: 2019-01-28 7:25 PM  - ErikEJ.SqlCeScripting version 3.5.2.80
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
1,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:43.626',250,9,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
2,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:40.539',250,9,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
3,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:40.536',250,9,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
4,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:43.624',250,8,15,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
5,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:40.833',250,8,14,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
6,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:40.436',250,8,13,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
7,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:43.626',250,8,12,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
8,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:43.625',250,8,11,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
9,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:43.627',250,8,10,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
10,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:43.632',250,8,9,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
11,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:43.647',250,8,8,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
12,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:43.625',250,8,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
13,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:40.746',250,8,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
14,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:40.671',250,8,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
15,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:40.939',250,8,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
16,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:40.559',250,8,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
17,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:40.687',250,8,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
18,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:40.711',250,9,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
19,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:40.804',250,8,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
20,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:43.625',250,9,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
21,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:41.709',250,9,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
22,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:40.705',250,10,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
23,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:43.626',250,10,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
24,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:40.966',250,10,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
25,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:41.744',250,10,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
26,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:40.995',250,10,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
27,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:42.105',250,10,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
28,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:43.632',250,10,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
29,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:43.624',250,10,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
30,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:42.111',250,9,15,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
31,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:43.630',250,9,14,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
32,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:43.632',250,9,13,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
33,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:44.428',250,9,12,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
34,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:44.438',250,9,11,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
35,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:44.441',250,9,10,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
36,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:44.439',250,9,9,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
37,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:45.410',250,9,8,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
38,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:45.413',250,9,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
39,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:45.519',250,9,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
40,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:45.429',250,8,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
41,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:45.479',250,7,15,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
42,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:45.938',250,7,14,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
43,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:45.603',250,6,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
44,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:45.937',250,6,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
45,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.254',250,6,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
46,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.291',250,6,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
47,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.154',250,6,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
48,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.129',250,6,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
49,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.155',250,6,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
50,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.160',250,6,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
51,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.167',250,5,15,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
52,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.127',250,5,14,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
53,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.128',250,5,13,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
54,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.128',250,5,12,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
55,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.276',250,5,11,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
56,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.241',250,5,10,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
57,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.276',250,5,9,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
58,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.211',250,5,8,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
59,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.806',250,5,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
60,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:47.101',250,6,8,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
61,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.230',250,6,9,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
62,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.224',250,6,10,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
63,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.649',250,6,11,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
64,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.649',250,7,13,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
65,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.357',250,7,12,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
66,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:48.448',250,7,11,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
67,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.911',250,7,10,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
68,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:47.096',250,7,9,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
69,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.965',250,7,8,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
70,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.972',250,7,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
71,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:47.095',250,7,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
72,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.276',250,10,8,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
73,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.227',250,7,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
74,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.805',250,7,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
75,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.806',250,7,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
76,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.806',250,7,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
77,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.806',250,7,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
78,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:47.050',250,6,15,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
79,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:47.100',250,6,14,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
80,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.270',250,6,13,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
81,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.217',250,6,12,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
82,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.219',250,7,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
83,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.953',250,10,9,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
84,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.913',250,10,10,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
85,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.539',250,10,11,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
86,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.791',250,14,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
87,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.658',250,14,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
88,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.840',250,14,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
89,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.981',250,14,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
90,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.647',250,14,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
91,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.913',250,14,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
92,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.913',250,14,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
93,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.648',250,14,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
94,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.507',250,13,15,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
95,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.512',250,13,14,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
96,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.650',250,13,13,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
97,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.553',250,13,12,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
98,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.821',250,13,11,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
99,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.928',250,13,10,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
100,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.403',250,13,9,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
101,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.960',250,13,8,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
102,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.840',250,13,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
103,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.928',250,14,8,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
104,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.929',250,14,9,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
105,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.734',250,14,10,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
106,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.500',250,14,11,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
107,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.824',250,15,13,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
108,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.797',250,15,12,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
109,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.744',250,15,11,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
110,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.839',250,15,10,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
111,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.726',250,15,9,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
112,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.696',250,15,8,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
113,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.840',250,15,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
114,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.840',250,15,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
115,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.726',250,13,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
116,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.840',250,15,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
117,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.824',250,15,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
118,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.783',250,15,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
119,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.859',250,15,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
120,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.824',250,15,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
121,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.825',250,14,15,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
122,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.945',250,14,14,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
123,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.910',250,14,13,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
124,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.910',250,14,12,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
125,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.869',250,15,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
126,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.999',250,5,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
127,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:52.007',250,13,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
128,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.994',250,13,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
129,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:52.046',250,11,12,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
130,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:52.158',250,11,11,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
131,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:52.734',250,11,10,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
132,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:52.732',250,11,9,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
133,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:52.746',250,11,8,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
134,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.375',250,11,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
135,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.071',250,11,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
136,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.455',250,11,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
137,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.306',250,11,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
138,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.202',250,11,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
139,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.205',250,11,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
140,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.306',250,11,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
141,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.206',250,11,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
142,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.306',250,10,15,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
143,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.311',250,10,14,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
144,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.307',250,10,13,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
145,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.311',250,10,12,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
146,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.306',250,11,13,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
147,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.320',250,11,14,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
148,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.205',250,11,15,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
149,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:52.890',250,12,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
150,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:52.893',250,13,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
151,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:54.102',250,13,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
152,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.711',250,13,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
153,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.534',250,12,15,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
154,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.060',250,12,14,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
155,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:54.218',250,12,13,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
156,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:54.486',250,12,12,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
157,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.476',250,12,11,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
158,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.648',250,13,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
159,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:54.770',250,12,10,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
160,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.843',250,12,8,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
161,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.757',250,12,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
162,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.973',250,12,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
163,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.121',250,12,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
164,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.214',250,12,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
165,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.121',250,12,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
166,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:54.492',250,12,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
167,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.405',250,12,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
168,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.632',250,12,9,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
169,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.922',250,5,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
170,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.487',250,5,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
171,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.599',250,5,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
172,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.595',250,4,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
173,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.097',250,4,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
174,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.678',250,4,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
175,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.951',250,4,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
176,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.718',250,4,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
177,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.176',250,4,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
178,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.797',250,4,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
179,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.847',250,3,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
180,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.833',250,3,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
181,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.863',250,3,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
182,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.865',250,3,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
183,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.948',250,3,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
184,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.935',250,3,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
185,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.958',250,3,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
186,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.026',250,3,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
187,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.257',250,2,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
188,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.029',250,2,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
189,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.027',250,4,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
190,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.998',250,5,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
191,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.170',250,5,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
192,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.047',250,5,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
193,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.083',250,7,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
194,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.084',250,7,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
195,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.072',250,7,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
196,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.085',250,7,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
197,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.174',250,7,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
198,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.316',250,6,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
199,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.174',250,6,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
200,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.180',250,6,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
201,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.349',250,2,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
202,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.342',250,6,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
203,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.304',250,6,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
204,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.264',250,6,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
205,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.471',250,6,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
206,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.488',250,5,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
207,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.317',250,5,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
208,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.501',250,5,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
209,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.353',250,5,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
210,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.490',250,5,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
211,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.362',250,6,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
212,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.500',250,7,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
213,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.493',250,2,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
214,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.508',250,2,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
215,2,'2019-01-02 15:04:20.110','2019-01-02 15:04:31.243',250,2,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
216,2,'2019-01-02 15:04:20.110','2019-01-02 15:04:31.233',250,2,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
217,2,'2019-01-02 15:04:20.110','2019-01-02 15:04:31.301',250,2,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
218,2,'2019-01-02 15:04:20.110','2019-01-02 15:04:31.312',250,2,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
219,2,'2019-01-02 15:04:20.110','2019-01-02 15:04:31.337',250,1,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
220,2,'2019-01-02 15:04:20.110','2019-01-02 15:04:31.519',250,1,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
221,2,'2019-01-02 15:04:20.110','2019-01-02 15:04:31.497',250,1,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
222,2,'2019-01-02 15:04:20.110','2019-01-02 15:04:31.459',250,1,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
223,2,'2019-01-02 15:04:20.110','2019-01-02 15:04:31.483',250,0,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
224,2,'2019-01-02 15:04:20.110','2019-01-02 15:04:31.511',250,0,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
225,2,'2019-01-02 15:04:20.110','2019-01-02 15:04:31.515',250,0,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
226,2,'2019-01-02 15:04:20.110','2019-01-02 15:04:31.390',250,0,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
227,1,'2019-01-02 15:04:20.110','2019-01-02 15:04:28.683',250,1,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
228,1,'2019-01-02 15:04:20.110','2019-01-02 15:04:28.645',250,1,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
229,1,'2019-01-02 15:04:20.110','2019-01-02 15:04:28.657',250,0,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
230,1,'2019-01-02 15:04:20.110','2019-01-02 15:04:28.645',250,0,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
231,0,'2019-01-02 15:04:20.110','2019-01-02 15:04:27.417',250,0,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
232,2,'2019-01-02 15:04:20.110','2019-01-02 15:04:31.367',250,3,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
233,2,'2019-01-02 15:04:20.110','2019-01-02 15:04:31.312',250,3,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
234,2,'2019-01-02 15:04:20.110','2019-01-02 15:04:31.272',250,3,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
235,2,'2019-01-02 15:04:20.110','2019-01-02 15:04:31.143',250,3,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
236,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.363',250,2,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
237,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.374',250,2,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
238,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.375',250,1,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
239,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.488',250,1,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
240,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.551',250,1,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
241,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.627',250,1,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
242,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.555',250,1,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
243,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.550',250,1,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
244,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.554',250,2,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
245,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.558',250,1,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
246,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.556',250,0,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
247,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.557',250,0,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
248,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.566',250,0,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
249,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:35.188',250,0,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
250,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.981',250,0,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
251,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.751',250,0,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
252,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.717',250,0,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
253,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.699',250,0,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
254,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.688',250,1,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
255,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.523',250,15,14,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
256,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.604',250,7,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
257,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:54.696',250,0,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
258,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.639',250,3,12,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
259,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.846',250,3,11,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
260,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.847',250,3,10,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
261,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:54.116',250,3,9,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
262,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.851',250,3,8,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
263,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.213',250,3,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
264,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.843',250,3,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
265,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:54.812',250,3,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
266,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:54.631',250,3,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
267,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.081',250,3,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
268,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:54.947',250,3,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
269,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.326',250,3,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
270,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:54.872',250,3,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
271,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.331',250,2,15,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
272,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.127',250,2,14,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
273,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.114',250,2,13,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
274,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:54.725',250,2,12,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
275,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:54.727',250,3,13,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
276,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.058',250,3,14,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
277,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.327',250,3,15,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
278,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.060',250,4,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
279,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.058',250,5,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
280,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.889',250,5,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
281,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.211',250,5,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
282,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.848',250,4,15,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
283,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.852',250,4,14,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
284,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.892',250,4,13,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
285,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.351',250,4,12,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
286,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.885',250,4,11,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
287,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.909',250,2,11,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
288,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.726',250,4,10,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
289,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.834',250,4,8,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
290,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.762',250,4,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
291,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.766',250,4,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
292,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.445',250,4,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
293,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.739',250,4,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
294,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.740',250,4,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
295,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.740',250,4,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
296,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.497',250,4,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
297,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.784',250,4,9,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
298,3,'2019-01-02 15:04:20.110','2019-01-02 15:04:34.595',250,7,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
299,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.762',250,2,10,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
300,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.778',250,2,8,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
301,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.740',250,1,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
302,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.711',250,1,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
303,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.808',250,0,15,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
304,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.844',250,0,14,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
305,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.782',250,0,13,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
306,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.788',250,0,12,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
307,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.713',250,0,11,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
308,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.821',250,0,10,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
309,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:57.482',250,0,9,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
310,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.907',250,0,8,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
311,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:57.477',250,0,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
312,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:57.854',250,0,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
313,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:57.841',250,0,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
314,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:57.510',250,0,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
315,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:57.742',250,0,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
316,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:57.740',250,0,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
317,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:57.853',250,0,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
318,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:57.856',250,1,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
319,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:57.900',250,1,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
320,4,'2019-01-02 15:04:20.110','2019-01-02 15:05:00.840',250,1,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
321,4,'2019-01-02 15:04:20.110','2019-01-02 15:05:00.847',250,1,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
322,4,'2019-01-02 15:04:20.110','2019-01-02 15:05:00.842',250,2,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
323,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:59.631',250,2,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
324,4,'2019-01-02 15:04:20.110','2019-01-02 15:05:00.556',250,2,5,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
325,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:58.371',250,2,4,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
326,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:59.628',250,2,3,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
327,4,'2019-01-02 15:04:20.110','2019-01-02 15:05:00.655',250,2,2,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
328,4,'2019-01-02 15:04:20.110','2019-01-02 15:05:00.655',250,2,1,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
329,4,'2019-01-02 15:04:20.110','2019-01-02 15:05:00.844',250,2,0,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
330,4,'2019-01-02 15:04:20.110','2019-01-02 15:05:00.842',250,2,9,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
331,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:59.628',250,1,15,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
332,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:59.624',250,1,13,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
333,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:56.740',250,1,12,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
334,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:55.105',250,1,11,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
335,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:53.957',250,1,10,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
336,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:51.898',250,1,9,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
337,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.984',250,1,8,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
338,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:50.828',250,1,7,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
339,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:46.313',250,1,6,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
340,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:43.629',250,1,14,1);
INSERT INTO [Tiles] ([TileId],[ZoomLevel],[CreationTimestamp],[CompletionTimestamp],[TileSize],[X],[Y],[MapId]) VALUES (
341,4,'2019-01-02 15:04:20.110','2019-01-02 15:04:40.716',250,15,15,1);
