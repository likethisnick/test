import { useEffect, useState } from 'react'
import './App.css'
import CreateQuestionForm from './myComponents/createQuestionForm'
import { createQuestion, fetchNotes } from './services/questions'
import Note from './myComponents/Note'
import Filters from './myComponents/Filters'
import { BrowserRouter, Link, Route, Routes } from 'react-router-dom'
import NavBar from './myComponents/Navbar'
import Blog from './Blog'
import SurvefyNavigate from './myComponents/SurvefyNavigate'


function App() {
  const[notes, setNotes] = useState([]);
  const[filter, setFilter] = useState({
    search: "",
    sortItem: "date",
    sortOrder: "desc"
  })

  useEffect(() => {
    const fetchData = async () => {
      let notes = await fetchNotes(filter);
      setNotes(notes);
    }

    fetchData();
  }, [filter]);

  const onCreate= async (note) => {
      await createQuestion(note);
      let notes = await fetchNotes(filter);
      setNotes(notes);
  }

  return (
    
    <div>
      <section>
      <SurvefyNavigate/>
      <NavBar/>
      </section>

    <section className="section">
      <div>
        <SurvefyNavigate/>
        <CreateQuestionForm onCreate ={onCreate}/>
          <Filters filter={filter} setFilter={setFilter}/>
      </div>
    


    <ul className="ul">
      <li>
        <Note title="a" description="b"/>
      </li>
        {notes.map((n) => (
          
            <li key={n.id}>
              <ul></ul>
                <Note title={n.title} description={n.description} createdAt={n.createdAt}/>
            </li>
        ))}
      </ul>

    </section>
    </div>
  )
}

export default App
