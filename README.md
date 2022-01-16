# meiosis-unity-animation

A short game-jam-like-project to create a procedural animation of <a href="https://en.wikipedia.org/wiki/Meiosis"><B>cell meiosis</B></A>

This project was originall done under pretty tight time constraints 

* Needs to be done in 10-12 hours
* Should not include any paid assets or proprietary code
* The core loop or flow should be understandable to a non-coder
* Flow should closely follow the whiteboard plan

You can see a video of the program running (with me controlling the janky camera) <A HREF="https://youtu.be/OLllRb2Odls"> here.</A>

<B>Code Architecture</B>

* The main cell has a Finite State Machine (FSM) which is based upon some course work from Jason Weimann
(<a href="https://www.youtube.com/c/Unity3dCollege"><B>link to YouTube channel</B></A>)
* Within each state (set up in OnEnter), we sequence a number of events like the cell growing, based upon a timer
* At the end of each phase (in OnExit) we clear all timers from the state machine rady for the next phase.
* Cells inrernally have a counter 

<B>Other Notes</B>

I set out to use this as a learning opportunity but time was too short to write much great code. The main new thing for me was the
mesh deformation needed to divide a call. I had seen a few tutorials and videos but never really tried it; it turned out to be much
easier than I thought and only required one short script. That is also available in a separate repo:
<A HREF="https://github.com/iangiblin/unity-mesh-deformation-demo"><B>unity mesh deformation demo</B></A>
