import bg from "../assets/bg-image.png";
import flashcards from "../assets/flashcards.jpg";

// Home page
const Home = () => {
  return (
    <div className="pb-16 md:pb-0">
      {/* Hero / Header  */}
      <div className="relative h-[275px] xl:h-[500px]">
        <img src={bg} alt="people studying" className="h-full w-full" />
        <div className="absolute bg-opacity-[.57] w-4/6 font-bold px-3 bg-accent rounded-r-3xl top-20">
          <div className="lg:hidden">
            <p className="text-center text-4xl px-2 text-white">Quiz Lit</p>
            <p className="text-xl text-right text-white leading-none">
              Master Your Studies
            </p>
          </div>
          <div className="hidden lg:block">
            <p className="text-center xl:text-right text-4xl xl:text-6xl text-white pb-5 pt-2 [text-shadow:_0_3px_0_rgb(7_0_0_/_40%)]">
              Quiz Lit - Master Your Studies
            </p>
          </div>
        </div>
        <div className="bg-opacity-[.65] rounded-l-2xl absolute right-0 text-black p-3 bg-secondary xl:text-5xl md:text-3xl text-lg font-bold top-40 xl:top-60 flex w-5/6">
          <p>Create, Study, and Share Flashcards</p>
        </div>
        <br />
      </div>

      {/* Info / CTA */}
      <div className="px-4 py-16 mx-auto sm:max-w-xl md:max-w-full lg:max-w-screen-xl md:px-24 lg:px-8 lg:py-20">
        <div className="flex flex-col items-center justify-between w-full mb-10 lg:flex-row">
          <img
            alt="flashcards"
            src={flashcards}
            className="border-8 border-accent w-[200px] h-[200px] lg:w-[450px] lg:h-[450px] rounded-full drop-shadow-[_0_3px_0_rgb(7_0_0_/_40%)]"
          />
          <div className="w-80 lg:h-[450px] lg:w-6 max-w-xl mb-6 mt-6 lg:mt-0 lg:mx-6 bg-accent h-[50%] rounded-full">
            &nbsp;{" "}
          </div>

          <div className="mb-16 lg:mb-0 lg:max-w-lg lg:pr-5">
            <div className="max-w-xl mb-6">
              <h2 className="font-sans text-3xl sm:mt-0 mt-6 font-bold tracking-tight text-black sm:text-4xl sm:leading-none max-w-lg mb-6">
                Quiz Lit - A powerful tool 🔥
              </h2>
              <p className="text-black text-2xl md:text-3xl">
                Quiz Lit is a powerful online tool that allows students to
                create, generate, and study flashcards. With Quiz Lit, you can
                easily create flashcards for any subject, generate flashcards
                from existing sets, and study them to improve your knowledge.
                Share your flashcards with others and collaborate on learning
                together.
              </p>
            </div>
            <div className="flex items-center space-x-3">
              <a href="/comingsoon" />
              <a className="text-center flex object-cover sm:mr-64 mr-32 object-top items-center text-white border border-2 justify-center w-full sm:px-10 py-4 leading-6 bg-black rounded-lg font-black">
                Master your subjects with Quiz Lit today!
              </a>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Home;
