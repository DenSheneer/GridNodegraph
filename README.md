# GridNodegraph
A tool for creating nodegraphs that are tile/grid based

# HOW TO USE
Open this unity project on unity hub. 
(the unity version you use doesn't matter a lot, as long as it's later or the same as this project's)

In unity, open the obstaclePainter scene.
In the inspector hierarchy, select the ObstaclePainter object.
While in sceneview you can "paint" obstacles.
It's also possible to place models into the scene, as long as these have a collider and are on the "obstacle" layer.
Make sure the coordinates of the bottom left and top right corners are >= 0 and are correct.

When done painting/placing obstacles, enter playmode.
Click the "export" button to create a JSON file in the resource folder.

This file contains the nodes that are covered by obstacles.
You can use this file in any way you want. For example in a C++ project.
