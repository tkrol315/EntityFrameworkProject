# EntityFrameworkProject
The rateQuestion and rateAnswer endpoints accept the following parameters: 'Like', 'Dislike', and 'UndoRating'.  
If a user tries to rate a question or answer more than once in a row, with the same parameter used, the application will return null.  
However, if the user changes their rating or undoes it before trying to rate again, the application will work as expected.
