import { useEffect, useState } from 'react'
import './App.css'
import CreateQuestionForm from './myComponents/createQuestionForm'
import { createQuestion, fetchNotes } from './services/questions'
import Note from './myComponents/Note'
import Filters from './myComponents/Filters'
import { BrowserRouter, Link, Route, Routes } from 'react-router-dom'
import Blog from './Blog'
import SurvefyNagivate from './myComponents/SurvefyNavigate'
import { Box, CssBaseline } from '@mui/material'
import Home from './Home'
import AppTheme from '../shared-theme/AppTheme'
import SignIn from './myComponents/SignIn'
import AuthorizeView from './myComponents/AuthorizeView'
import CreateSurvey from './CreateSurvey'


function App(props) {
  return (
    <AppTheme {...props}>
      <CssBaseline enableColorScheme />
      <AuthorizeView>
      <BrowserRouter>
        <Routes>
          <Route path="/blog" element={<Blog />} />
        </Routes>
        <Routes>
          <Route path="/" element={<Home />} />
        </Routes>
        <Routes>
          <Route path="/signin" element={<SignIn />} />
        </Routes>
        <Routes>
          <Route path="/createsurvey" element={<CreateSurvey />} />
        </Routes>
      </BrowserRouter>
      </AuthorizeView>
    </AppTheme>
  )
}

export default App
